using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using System.Linq;
using System;

namespace Buffs
{
    internal class FioraDance : IBuffGameScript
    {
        private Buff thisBuff;
        private AttackableUnit initialTarget;
        private ObjAIBase owner;

        private float time;

        private readonly int MAX_STRIKES = 5;
        private int currentStrike = 1;

        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING,
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            owner = ownerSpell.CastInfo.Owner;
            initialTarget = ownerSpell.CastInfo.Targets[0].Unit;

            SetCastingStatuses(true);
            SealSpells(true);

            Strike();
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            SetCastingStatuses(false);
            SealSpells(false);
        }

        public void OnUpdate(float diff)
        {
            time += diff;
            owner.SetTargetUnit(null, true);
            if (time >= 350.0f)
            {
                Strike();
                time = 0f;
            }
        }

        private void SetCastingStatuses(bool casting)
        {
            SetStatus(owner, StatusFlags.CanMove, !casting);
            SetStatus(owner, StatusFlags.CanAttack, !casting);
            SetStatus(owner, StatusFlags.Ghosted, casting);
            SetStatus(owner, StatusFlags.Targetable, !casting);
        }

        private void SealSpells(bool seal)
        {
            for (int i = 0; i < 4; i++)
            {
                SealSpellSlot(owner, SpellSlotType.SpellSlots, i, SpellbookType.SPELLBOOK_CHAMPION, seal);
            }
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_SUMMONER, seal);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_SUMMONER, seal);
        }

        private void Strike()
        {
            AttackableUnit currentTarget = DetermineCurrentTarget();

            if (currentTarget == null || currentTarget.IsDead)
            {
                thisBuff.DeactivateBuff();
                return;
            }

            currentStrike++;
            FaceDirection(currentTarget.Position, owner, true);
            SpellCast(owner, 1, SpellSlotType.ExtraSlots, false, currentTarget, Vector2.Zero);

            if (currentStrike > MAX_STRIKES)
            {
                thisBuff.DeactivateBuff();
            }
        }

        private AttackableUnit DetermineCurrentTarget()
        {
            AttackableUnit currentTarget;

            if (currentStrike == 1 || currentStrike == 5)
            {
                if (initialTarget.IsDead)
                {
                    return TryGetRandomTargetInCastRange();
                }
                else
                {
                    return initialTarget;
                }
            }
            else
            {
                currentTarget = TryGetRandomTargetInCastRange();
                if (currentTarget == null)
                {
                    return initialTarget;
                }
            }

            return currentTarget;
        }

        private AttackableUnit TryGetRandomTargetInCastRange()
        {
            return GetChampionsInRange(owner.Position, 400, true)
                .Where(c => !c.Team.Equals(owner.Team)) // Only enemy champions
                .OrderBy(a => Guid.NewGuid()) // Randomize the order
                .FirstOrDefault((AttackableUnit)null);
        }
    }
}
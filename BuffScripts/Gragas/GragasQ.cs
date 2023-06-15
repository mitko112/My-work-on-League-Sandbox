using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;
using Roy_T.AStar.Primitives;
using System.Numerics;

namespace Buffs
{
    class GragasQ : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Spell ThisSpell;
        Buff ThisBuff;
        bool hascasted = false;
        ObjAIBase Owner;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ThisSpell = ownerSpell;
            ThisBuff = buff;

            Owner = ownerSpell.CastInfo.Owner;
            ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("GragasQ"), TargetExecute);
        }
        public void TargetExecute(Spell spell)
        {
            hascasted = true;
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (hascasted == false)
            {
                ownerSpell.CastInfo.Owner.SetSpell("GragasQ", 0, true);
                var Position = new Vector2(ownerSpell.CastInfo.TargetPositionEnd.X, ownerSpell.CastInfo.TargetPositionEnd.Z);
                var ap = Owner.Stats.AbilityPower.Total * 0.3f;
                var damage = (40 * (ThisSpell.CastInfo.SpellLevel)) + ap;
                var enemies = GetUnitsInRange(Position, 250f, true);
                foreach (var enemy in enemies)
                {
                    enemy.TakeDamage(ThisSpell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                }
            }
            SealSpellSlot(ownerSpell.CastInfo.Owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
        }



        public void OnUpdate(float diff)
        {
        }
    }
}
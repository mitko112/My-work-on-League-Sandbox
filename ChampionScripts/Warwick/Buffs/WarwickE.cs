using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;

namespace Buffs
{
    internal class BloodScent : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public SpellSector Bloodscent;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Bloodscent = ownerSpell.CreateSpellSector(new SectorParameters
            {
                BindObject = ownerSpell.CastInfo.Owner,
                Length = 1250f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });

            ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, TargetExecute, false);

        }
            public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
            {

            var owner = spell.CastInfo.Owner;
            if (target.Stats.CurrentHealth < target.Stats.HealthPoints.Total * 0.5f)
            {
                AddBuff("BloodScentTicker", 1f, 1, spell, target, owner, false);
                AddBuff("BloodScentMoveSpeed", 1f, 1, spell, owner, owner, false);
                


            }

        }

            }


        }
    





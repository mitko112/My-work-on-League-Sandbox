using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using System.Collections.Generic;
using GameServerCore.Domain.GameObjects.Spell.Missile;

namespace Buffs
{
    class NasusR : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.RENEW_EXISTING,
            MaxStacks = 1,
            IsHidden = false
        };
        

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IObjAiBase Owner;
        public ISpellSector NasusRAOE;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Owner = ownerSpell.CastInfo.Owner;
            ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, TargetExecute, false);

           
            var APratio = Owner.Stats.AbilityPower.Total * 0.5f;
            var HealthBuff = 150f + 150f * ownerSpell.CastInfo.SpellLevel + APratio;

            StatsModifier.Size.PercentBonus = StatsModifier.Size.PercentBonus + 0.3f;
            StatsModifier.HealthPoints.BaseBonus += HealthBuff;

            StatsModifier.Range.FlatBonus = StatsModifier.Size.FlatBonus + 50f;
            StatsModifier.HealthPoints.BaseBonus += HealthBuff;


            unit.AddStatModifier(StatsModifier);
            unit.Stats.CurrentHealth += HealthBuff;

        

            NasusRAOE = ownerSpell.CreateSpellSector(new SectorParameters
            {
                BindObject = ownerSpell.CastInfo.Owner,
                Length = 400f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            float AP = Owner.Stats.AbilityPower.Total * 0.2f;
            float damage = 20f + (15 * spell.CastInfo.SpellLevel) + AP;

            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL,DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnSpellHit.RemoveListener(this);
            NasusRAOE.SetToRemove();
        }
        public void OnUpdate(float diff)
        {

        }
    }
}
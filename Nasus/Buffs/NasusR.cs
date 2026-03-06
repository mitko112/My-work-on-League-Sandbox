using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class NasusR : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        ObjAIBase Owner;
        SpellSector StormSector;

        private float StormDamageThisSecond;
        private float TotalBonusAD;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private StatsModifier BonusADModifier;

        private const float ConversionRatio = 0.06375f;
        private const float MaxBonusAD = 300f;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Owner = ownerSpell.CastInfo.Owner;

            float bonusHP = 300f + 150f * (ownerSpell.CastInfo.SpellLevel - 1);
            StatsModifier.Size.PercentBonus = 0.3f;
            StatsModifier.HealthPoints.BaseBonus = bonusHP;
            StatsModifier.Range.FlatBonus = 50f;

            unit.AddStatModifier(StatsModifier);

            
            unit.Stats.CurrentHealth += bonusHP;

            StormSector = ownerSpell.CreateSpellSector(new SectorParameters
            {
                BindObject = Owner,
                Length = 175f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = true,
                OverrideFlags =
                    SpellDataFlags.AffectEnemies |
                    SpellDataFlags.AffectMinions |
                    SpellDataFlags.AffectHeroes |
                    SpellDataFlags.AffectNeutral,
                Type = SectorType.Area
            });

            ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, OnStormHit, false);
        }

        public void OnStormHit(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            float maxHealthDamage = target.Stats.HealthPoints.Total * 0.03f;
            float damage = MathF.Min(maxHealthDamage, 240f);

            target.TakeDamage(
                Owner,
                damage,
                DamageType.DAMAGE_TYPE_MAGICAL,
                DamageSource.DAMAGE_SOURCE_SPELLAOE,
                false
            );

            StormDamageThisSecond += damage;
        }

        public void OnUpdate(float diff)
        {
            if (StormDamageThisSecond <= 0)
                return;

            float gainedAD = StormDamageThisSecond * ConversionRatio;
            StormDamageThisSecond = 0;

            TotalBonusAD += gainedAD;
            if (TotalBonusAD > MaxBonusAD)
                TotalBonusAD = MaxBonusAD;

            if (BonusADModifier != null)
                Owner.RemoveStatModifier(BonusADModifier);

            BonusADModifier = new StatsModifier();
            BonusADModifier.AttackDamage.FlatBonus = TotalBonusAD;
            Owner.AddStatModifier(BonusADModifier);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnSpellHit.RemoveListener(this);

            if (StormSector != null)
                StormSector.SetToRemove();

            if (BonusADModifier != null)
                Owner.RemoveStatModifier(BonusADModifier);
        }
    }
}
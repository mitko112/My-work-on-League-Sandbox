using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;

namespace Buffs
{
    class HecarimWVFX : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        ObjAIBase Owner;
        public SpellSector HecarimWAOE;
        Particle p;
        Particle p1;
        Particle p2;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Owner = ownerSpell.CastInfo.Owner;
            ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, TargetExecute, false);


            p = p = AddParticle(
    Owner,                  // source
    Owner,                  // bind to THIS object
    "Hecarim_Defile.troy",   // particle
    Owner.Position,          // initial position
    buff.Duration
);




            HecarimWAOE = ownerSpell.CreateSpellSector(new SectorParameters
            {
                BindObject = ownerSpell.CastInfo.Owner,
                Length = 525f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            float AP = Owner.Stats.AbilityPower.Total * 0.2f;

            float damage = 20 *(spell.CastInfo.SpellLevel) + AP;
            float heal = damage * 0.2f;
            float heali = 60f;

            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL,DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            Owner.Stats.CurrentHealth += heal;
            if (target is Minion && target is Monster & damage > 300)
            {
                Owner.Stats.CurrentHealth += heali;
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnSpellHit.RemoveListener(this);
            HecarimWAOE.SetToRemove();

            RemoveParticle(p);
            
        }
        public void OnUpdate(float diff)
        {

        }
    }
}
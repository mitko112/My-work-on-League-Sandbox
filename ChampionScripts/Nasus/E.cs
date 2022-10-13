using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace Spells
{
    public class NasusE : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true,


        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }
        Buff thisBuff;
        public SpellSector DamageSector;

    

       

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var targetPos = GetPointFromUnit(owner, 650.0f);
            SpellCast(owner, 1, SpellSlotType.ExtraSlots, targetPos, targetPos, false, Vector2.Zero);
            var spellpos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.6f;
            var initialdamage = 55*spell.CastInfo.SpellLevel  + ap;

            AddParticle(owner, null, "Nasus_Base_E_SpiritFire.troy", spellpos, lifetime: 5.0f);
            AddParticle(owner, null, "Nasus_Base_E_Staff_Swirl.troy", spellpos, lifetime: 5.0f);
            AddParticle(owner, null, "Nasus_Base_E_Warning.troy", spellpos, lifetime: 5.0f);
            AddParticle(owner, null, "Nasus_E_Green_Ring.troy", spellpos, lifetime: 5.0f);
            DamageSector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 400f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
                Lifetime = 5.0f
            });

            var units = GetUnitsInRange(SpellPos, 650f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == owner.Team || units[i] is BaseTurret || units[i] is ObjBuilding || units[i] is Inhibitor))
                {
                    units[i].TakeDamage(owner, initialdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                }
            }
        }

       
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            //var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.6f;
            //var damage = 55 + (spell.CastInfo.SpellLevel ) + ap;

             //target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            AddBuff("NasusE", 1f, 1, spell, target, owner);

        }
       
        public void OnUpdate(float diff)
        {
        }
    }
}

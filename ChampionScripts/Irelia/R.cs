using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
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
    public class IreliaTranscendentBladesSpell : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true
        
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {

         FaceDirection(end, owner);

        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
           {
            
                var owner = spell.CastInfo.Owner;
                var APratio = owner.Stats.AbilityPower.Total*0.5f;
                var ADratio = owner.Stats.AttackDamage.FlatBonus * 0.6f;
                var damage = 80*(spell.CastInfo.SpellLevel) + ADratio + APratio;
                

                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddParticleTarget(owner, owner, "irelia_ult_tar.troy", owner, lifetime: 1f);

               var units = GetUnitsInRange(owner.Position, 1000f, true);
               for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == owner.Team || units[i] is BaseTurret || units[i] is ObjBuilding || units[i] is Inhibitor))
                {
                    units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                    



                    missile.SetToRemove();
                    }
                }

            }


  
    }
}



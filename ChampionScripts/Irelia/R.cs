using System;
using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class IreliaTranscendentBladesSpell : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true
        
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        
         

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {

         FaceDirection(end, owner);

        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
           {
            
                var owner = spell.CastInfo.Owner;
                var APratio = owner.Stats.AbilityPower.Total*0.5f;
                var ADratio = owner.Stats.AttackDamage.FlatBonus * 0.6f;
                var damage = 80*(spell.CastInfo.SpellLevel) + ADratio + APratio;
                

                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddParticleTarget(owner, target, "irelia_ult_tar.troy", target, lifetime: 1f);

               var units = GetUnitsInRange(owner.Position, 1000f, true);
               for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == owner.Team || units[i] is IBaseTurret || units[i] is IObjBuilding || units[i] is IInhibitor))
                {
                    units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                    



                    missile.SetToRemove();
                    }
                }

            }


  
  public void OnSpellCast(ISpell spell)
{
}

public void OnSpellPostCast(ISpell spell)
{
}

public void OnSpellChannel(ISpell spell)
{
}

public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
{
}

public void OnSpellPostChannel(ISpell spell)
{
}

public void OnUpdate(float diff)
{
}
    }
}

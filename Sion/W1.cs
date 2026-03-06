
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;

namespace Spells
{
    public class SionWDetonate : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            spell.SetCooldown(4f, false);
        }


        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.9f;
            var damage = 70*spell.CastInfo.SpellLevel + ap;

            AddParticleTarget(owner, owner, "DeathsCaress_nova.troy", owner, 1f);
            
            
            var units = GetUnitsInRange(owner.Position, 550f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == owner.Team || units[i] is BaseTurret || units[i] is ObjBuilding || units[i] is Inhibitor))
                {
                    units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);

                    RemoveBuff(owner, "SionWSwitch");
                }
            }

            if (owner.HasBuff("SionWShield"))


            {

                
                RemoveBuff(owner, "SionWShield");



            }
        }

       
    }
}

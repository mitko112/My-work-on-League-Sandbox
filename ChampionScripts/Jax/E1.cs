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
    public class JaxCounterStrikeAttack : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };
        public void OnSpellPostCast(Spell spell)

        {
            var owner = spell.CastInfo.Owner as Champion;
            var AD = spell.CastInfo.Owner.Stats.AttackDamage.FlatBonus * 0.5f;
            var damage = 50 + spell.CastInfo.SpellLevel + AD;


            

            var units = GetUnitsInRange(owner.Position, 400f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == owner.Team || units[i] is BaseTurret || units[i] is ObjBuilding || units[i] is Inhibitor))
                {
                    units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                    AddParticleTarget(owner, units[i], "globalhit_orange_tar.troy", units[i], 1f);
                    AddParticleTarget(owner, units[i], "Counterstrike_cas.troy", units[i], 1f);
                    AddBuff("Stun", 1.5f, 1, spell, units[i], owner);

                    spell.SetCooldown(14f, false);

                    if (owner.HasBuff("JaxEDodge"))
                    {

                        RemoveBuff(owner, "JaxEDodge");
                        
                    }

                    if (owner.HasBuff("JaxESelfcast"))
                    {

                        RemoveBuff(owner, "JaxESelfcast");

                    }








                }

            }
        }
    }
}
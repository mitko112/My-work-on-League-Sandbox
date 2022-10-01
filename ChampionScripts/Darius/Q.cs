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
    public class DariusCleave : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var ad = spell.CastInfo.Owner.Stats.AttackDamage.FlatBonus * 0.5f;
            var damage = 70*spell.CastInfo.SpellLevel  + ad;

                    AddParticle(owner, null, "darius_Base_Q_tar.troy", owner.Position, direction: owner.Direction);
                    AddParticle(owner, null, "darius_Base_Q_aoe_cast.troy", owner.Position, direction: owner.Direction);
                    AddParticle(owner, null, "darius_Base_Q_aoe_cast_mist", owner.Position, direction: owner.Direction);
                    
                    AddParticle(owner, null, "darius_Base_Q_tar_inner.troy", owner.Position, direction: owner.Direction);
                    PlayAnimation(owner, "Spell1", 0.7f);


            var units = GetUnitsInRange(owner.Position, 425f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == owner.Team || units[i] is BaseTurret || units[i] is ObjBuilding || units[i] is Inhibitor))
                {
                    units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);

                    AddParticleTarget(owner, units[i], "darius_Base_Q_impact_spray.troy", units[i], 1f);
                }
            }

        }

        }
    
}

using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;

namespace Spells
{
    public class Tantrum : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            AddBuff("AmumuEPassive", 1, 1, spell, owner, owner, true);


            }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.5f;
            var damage = 75*(spell.CastInfo.SpellLevel)  + ap;

            AddParticleTarget(owner, owner, "globalhit_orange_tar.troy", owner, 1f);


            var units = GetUnitsInRange(owner.Position, 400f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == owner.Team || units[i] is BaseTurret || units[i] is ObjBuilding || units[i] is Inhibitor))
                {
                    units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                    AddParticleTarget(owner, units[i], "tantrum_tar.troy", units[i], 1f);
                }
            }

        }

        }
    }

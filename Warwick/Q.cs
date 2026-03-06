using System.Numerics;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Spells
{
    public class HungeringStrike : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
        }


        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var Hpratio = Target.Stats.HealthPoints.Total*0.08f;
            var APratio = owner.Stats.AbilityPower.Total;
            var damage = 75 * spell.CastInfo.SpellLevel + APratio + Hpratio;
            var heal = 60 * spell.CastInfo.SpellLevel;
 
            owner.Stats.CurrentHealth += heal;


            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

            AddParticleTarget(owner, Target, "HungeringStrike_tar.troy", Target, 1f, 1f);
        }

       
        }
    }

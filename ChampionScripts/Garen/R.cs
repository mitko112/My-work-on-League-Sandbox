using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Spells
{
    public class GarenR : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var percentMissingHP = new[] { 0.2857f, 0.3333f, 0.4f }[spell.CastInfo.SpellLevel - 1];
            var damage = 175f * spell.CastInfo.SpellLevel + percentMissingHP * (Target.Stats.HealthPoints.Total - Target.Stats.CurrentHealth);
            
            AddParticleTarget(owner, Target, "Garen_Base_R_Sword_Tar.troy", Target, lifetime: 1f);

            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

            AddParticle(owner, Target, "Garen_Base_R_Tar_Impact.troy", Target.Position, 1f);
            if (Target.IsDead)
            {
                AddParticleTarget(owner, Target, "Garen_Base_R_Champ_Kill.troy", Target, 1f);
                AddParticleTarget(owner, Target, "Garen_Base_R_Champ_Death.troy", Target, 1f);
            }

        }
    }
}

using LeagueSandbox.GameServer.API;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
namespace Spells
{
    public class BusterShot : ISpellScript
    {
        IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var APratio = owner.Stats.AbilityPower.Total;
            var damage = 300 * spell.CastInfo.SpellLevel + APratio;
            var trueCoords = GetPointFromUnit(owner, 600f);


            AddParticleTarget(owner, Target, "tristana_bustershot_tar", Target, 1f, 1f);
            AddParticleTarget(owner, owner, "BusterShot_cas.troy", owner, 1f, 1f);
            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            ForceMovement(Target, "RUN", trueCoords, 2200, 0, 0, 0);


        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell)
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
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class FizzQ1 : IBuffGameScript
    {
        
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };
        
        public IStatsModifier StatsModifier { get; private set; }

        private readonly IAttackableUnit target = Spells.FizzPiercingStrike._target;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            var time = 0.6f - ownerSpell.CastInfo.SpellLevel * 0.1f;
            var damage = 50f + ownerSpell.CastInfo.SpellLevel * 20f + unit.Stats.AbilityPower.Total * 0.6f;
            AddParticleTarget(owner, owner, "Fizz_PiercingStrike.troy", owner);
            AddParticleTarget(owner, target, "Fizz_PiercingStrike_tar.troy", target);
            var to = Vector2.Normalize(target.Position - unit.Position);

            var xy = unit as IObjAiBase;
            xy.SetTargetUnit(null);

            ForceMovement(unit, null, new Vector2(target.Position.X + to.X * 250f, target.Position.Y + to.Y * 250f), 800f + unit.Stats.MoveSpeed.Total * 0.6f, 0, 0, 0); ; ;
            target.TakeDamage(unit, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}

using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;

namespace Spells
{
    public class VayneCondemn : ISpellScript
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
            var ADratio = owner.Stats.AttackDamage.FlatBonus*0.5f;
            var damage = 45 * spell.CastInfo.SpellLevel + ADratio;
            var trueCoords = GetPointFromUnit(owner, 470f);


            
            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            ForceMovement(Target, "RUN", trueCoords, 2200, 0, 0, 0);


        }

        
    }
}
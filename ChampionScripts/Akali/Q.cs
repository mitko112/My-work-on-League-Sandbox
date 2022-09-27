

using System.Numerics;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Spells
{
    public class AkaliShadowDance : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };


        Spell Spell;
        AttackableUnit Target;




        public void OnSpellCast(Spell spell)
        {
            Spell = spell;
            var target = spell.CastInfo.Targets[0].Unit;
            Target = target;
        }

        public void OnSpellPostCast(Spell spell)

        {


            var owner = spell.CastInfo.Owner;
            
            var current = owner.Position;
            var to = Vector2.Normalize(Target.Position - current);
            var range = to * 800;

            var trueCoords = current + range;

            //TODO: Dash to the correct location (in front of the enemy IChampion) instead of far behind or inside them
            //ForceMovement(owner, target, "Spell4", 1000, 0, 0, 0, 200);
            ForceMovement(owner, "Spell4", Target.Position, 2200, 0, 0, 0);
            //ForceMovement(spell.CastInfo.Owner, "Spell4", trueCoords, 2200, 0, 0, 0);
            AddParticleTarget(owner, Target, "akali_shadowDance_tar", Target);


            ApiEventManager.OnMoveEnd.AddListener(owner, owner, ApplyEffects, true);
        }

        public void ApplyEffects(AttackableUnit target)
        {
            
            var owner = Spell.CastInfo.Owner;
            var bonusAd = owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue;
            var ap = owner.Stats.AbilityPower.Total * 0.9f;
            var damage = 200 + Spell.CastInfo.SpellLevel * 150 + bonusAd + ap;
            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL,
                DamageSource.DAMAGE_SOURCE_SPELL, false);
        }

 
    }
}

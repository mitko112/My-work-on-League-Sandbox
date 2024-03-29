using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeaguePackets.Game.Common.CastInfo;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class DariusExecute : ISpellScript
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
            var ad = owner.Stats.AttackDamage.FlatBonus * 1.5f;
            var damage = 160 * spell.CastInfo.SpellLevel + ad;

            if (Target.HasBuff("DariusHemoMarker"))

            {
                
                
                    damage *= 1 + (Target.GetBuffWithName("DariusHemoMarker").StackCount * 0.2f);
                
            }




                Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);

            AddParticleTarget(owner, Target, "darius_Base_R_tar.troy", Target, 1f, 1f);

            if (Target.IsDead)
            {
                spell.SetCooldown(0.1f, false);

                AddBuff("DariusRRefresh",20f,1, spell, owner, owner);
            }


        }
    }
}

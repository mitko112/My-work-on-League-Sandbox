
using GameServerCore.Enums;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
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
using System.Linq;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
namespace Spells
{
    public class PantheonRJump : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            NotSingleTargetSpell = true,
            TriggersSpellCasts = true,
            ChannelDuration = 2f,
        };

        ObjAIBase Owner;


        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Owner = owner;
        }

        public void OnSpellCast(Spell spell)
        {
            AddParticleTarget(Owner, Owner, "Pantheon_Base_R_cas.troy", Owner, flags: 0);

        }


        public void OnSpellPostChannel(Spell spell)
        {
            var spellpos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            SpellCast(Owner, 1, SpellSlotType.ExtraSlots, spellpos, spellpos, false, Vector2.Zero);
            AddParticle(Owner, null, "Pantheon_Base_R_indicator_green.troy", spellpos, 2.7f);
        }


        public class PantheonRFall : ISpellScript
        {
            public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
            {
                NotSingleTargetSpell = true,
                TriggersSpellCasts = true,
                ChannelDuration = 2f
            };

            private ObjAIBase Owner;
            private Vector2 TargetPos;

            public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
            {
                Owner = owner;
                TargetPos = end;
            }

            public void OnSpellCast(Spell spell)
            {
                // Channel animation
                AddParticleTarget(Owner, Owner, "Pantheon_Base_R_cas.troy", Owner);

                // Landing indicator
                AddParticle(Owner, null, "Pantheon_Base_R_indicator_green.troy", TargetPos, 2.7f);
            }

            public void OnSpellPostChannel(Spell spell)
            {
                // Teleport
                Owner.TeleportTo(TargetPos.X, TargetPos.Y);

                // Impact VFX
                AddParticle(Owner, null, "Pantheon_Base_R_aoe_explosion.troy", TargetPos);

                float ap = Owner.Stats.AbilityPower.Total;
                float baseDamage = 100 + (300 * spell.CastInfo.SpellLevel) + ap;

                var enemies = GetUnitsInRange(TargetPos, 700f, true);

                foreach (var enemy in enemies)
                {
                    float dist = Vector2.Distance(enemy.Position, TargetPos);
                    float dmg = baseDamage;

                    if (dist > 580f) dmg *= 0.5f;
                    else if (dist > 464f) dmg *= 0.6f;
                    else if (dist > 348f) dmg *= 0.7f;
                    else if (dist > 232f) dmg *= 0.8f;
                    else if (dist > 116f) dmg *= 0.9f;

                    enemy.TakeDamage(
                        Owner,
                        dmg,
                        DamageType.DAMAGE_TYPE_MAGICAL,
                        DamageSource.DAMAGE_SOURCE_SPELLAOE,
                        false
                    );
                }
            }

            public void OnUpdate(float diff) { }
        }
    }
}
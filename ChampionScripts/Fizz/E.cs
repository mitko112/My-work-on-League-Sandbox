using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;

namespace Spells
{
    public class FizzJump : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = false,
            NotSingleTargetSpell = true
            // TODO
        };
        float ticks = 0;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            if (!owner.HasBuff("FizzBuffer") && !owner.HasBuff("FizzTrickSlam"))
            {
                var trueCoords = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
                var startPos = owner.Position;
                var to = trueCoords - startPos;
                if (to.Length() > 400)
                {
                    trueCoords = GetPointFromUnit(owner, 400f);
                }

                ForceMovement(owner, null, trueCoords, 1200, 0, 0, 0);
                PlayAnimation(owner, "Spell3a", 0.75f);
                AddParticleTarget(owner, owner, "Fizz_Jump_cas.troy", owner);
                AddParticleTarget(owner, owner, "Fizz_Jump_WeaponGlow.troy", owner, bone: "BUFFBONE_GLB_WEAPON_1");
                AddBuff("FizzBuffer", 0.75f, 1, spell, owner, owner);
                AddBuff("FizzTrickSlam", 1.1f, 1, spell, owner, owner);

            }
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }





    public class FizzJumpTwo : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = false,
            NotSingleTargetSpell = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var trueCoords = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var startPos = owner.Position;
            var to = trueCoords - startPos;
            if (to.Length() > 400)
            {
                trueCoords = GetPointFromUnit(owner, 400f);
            }

            ForceMovement(owner, "Spell1", trueCoords, 1200, 0, 0, 0);
            AddParticleTarget(owner, owner, "Fizz_Jump_cas.troy", owner);
            AddParticleTarget(owner, owner, "Fizz_Jump_WeaponGlow.troy", owner, bone: "BUFFBONE_GLB_WEAPON_1");

        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }





    public class FizzTrickSlam : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = false,
            NotSingleTargetSpell = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var trueCoords = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var startPos = owner.Position;
            var to = trueCoords - startPos;
            if (to.Length() > 400)
            {
                trueCoords = GetPointFromUnit(owner, 400f);
            }

            ForceMovement(owner, "Spell3a", trueCoords, 1200, 0, 0, 0);
            AddParticleTarget(owner, owner, "Fizz_Jump_cas.troy", owner);
            AddParticleTarget(owner, owner, "Fizz_Jump_WeaponGlow.troy", owner, bone: "BUFFBONE_GLB_WEAPON_1");

        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}


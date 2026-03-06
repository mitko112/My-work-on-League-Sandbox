using System.Numerics;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class InfiniteDuress : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
            // TODO
        };

        Spell Spell;
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;

            Spell = spell;
        }

        public void OnSpellPostCast(Spell spell)
        {


            var owner = spell.CastInfo.Owner;

            ApiEventManager.OnMoveEnd.AddListener(this, owner, OnMoveEnd, true);
            var target = spell.CastInfo.Targets[0].Unit;
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var to = Vector2.Normalize(new Vector2(target.Position.X, target.Position.Y) - current);
            var range = to * 700;
            var trueCoords = current + range;

            AddBuff("InfiniteDuress", 1.8f, 1, spell, target, owner, false);
            AddBuff("Suppression", 1.8f, 1, spell, target, owner, false);

            
            PlayAnimation(owner, "Spell4_loop", 0.7f);
            ForceMovement(owner, "Spell4", target.Position, 2200, 0, 0, 0);



            AddParticleTarget(owner, Target, "InfiniteDuress_tar.troy", Target, 1f, 1f);

        }
        public void OnMoveEnd(AttackableUnit owner)
        {
            CreateTimer(1.8f, () => PlayAnimation(owner, "Spell4_Winddown", 0.7f));

        }
        }

        }




    

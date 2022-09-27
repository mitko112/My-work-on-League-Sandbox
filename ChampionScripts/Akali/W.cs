using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;

namespace Spells
{
    public class AkaliSmokeBomb : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
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
            var initialCastPos = owner.Position;
            AddParticle(owner, null, "akali_smoke_bomb_tar.troy", initialCastPos, 8f);
            AddParticle(owner, null, "akali_smoke_bomb_tar_team_green.troy", initialCastPos, 8f);

            /*
              TODO: Display green border (akali_smoke_bomb_tar_team_green.troy) for the own team,
              display red border (akali_smoke_bomb_tar_team_red.troy) for the enemy team
              Currently only displaying the green border for everyone.
              -Add invisibility.
            */
        }

        }
    }



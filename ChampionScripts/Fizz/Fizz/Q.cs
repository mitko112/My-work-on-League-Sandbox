using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;

namespace Spells
{
    public class FizzPiercingStrike : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        public static AttackableUnit _target = null;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            //here's nothing yet
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
            //here's empty
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            _target = target;
            AddBuff("FizzQ1", 0.395f - spell.CastInfo.SpellLevel * 0.012f, 1, spell, owner, owner);
            PlayAnimation(owner, "SPELL1", 0.395f, 0f , -10f);

        }

        public void OnSpellCast(Spell spell)
        {
            //here's empty, maybe will add some functions?
        }

        public void OnSpellPostCast(Spell spell)
        {
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

    public class FizzQ1 : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {

        };

        public static AttackableUnit _target = null;

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

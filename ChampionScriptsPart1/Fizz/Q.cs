using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class FizzPiercingStrike : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        public static IAttackableUnit _target = null;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            //here's nothing yet
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
            //here's empty
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            _target = target;
            AddBuff("FizzQ1", 0.395f - spell.CastInfo.SpellLevel * 0.012f, 1, spell, owner, owner);
            PlayAnimation(owner, "SPELL1", 0.395f, 0f , -10f);

        }

        public void OnSpellCast(ISpell spell)
        {
            //here's empty, maybe will add some functions?
        }

        public void OnSpellPostCast(ISpell spell)
        {
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

    public class FizzQ1 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {

        };

        public static IAttackableUnit _target = null;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {

        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {

        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {

        }

        public void OnSpellPostCast(ISpell spell)
        {
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

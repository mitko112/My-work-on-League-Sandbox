using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class XenZhaoComboTarget : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }
        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("XenZhaoComboAuto", 5.0f, 1, spell, owner, owner,false);
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
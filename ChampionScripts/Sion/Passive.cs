using GameServerCore.Domain.GameObjects;

using GameServerCore.Domain.GameObjects.Spell;

using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace CharScripts
{
    public class CharScriptSion : ICharScript
    {
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        AddBuff("Size", 3.0f, 1, spell, owner, owner,true);
            
        }
        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}

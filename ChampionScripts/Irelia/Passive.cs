using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace CharScripts
{
    public class CharScriptIrelia : ICharScript
    {

        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            AddBuff("IreliaPassive", 99999f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);
        }
    }
}
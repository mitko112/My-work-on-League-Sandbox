using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace CharScripts
{

    public class CharScriptNasus : ICharScript

    {
        
        public void OnActivate(ObjAIBase owner, Spell spell)

        {
            
            AddBuff("NasusLifeSteal", 25000f, 1, spell, owner,owner, true);

        }




    }
}
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace CharScripts
{
     
    public class CharScriptDrMundo : ICharScript

    {
        
        public void OnActivate(ObjAIBase owner, Spell spell)

        {

           
            AddBuff("DrMundoPassive", 2f, 1, spell, owner, owner, true);

        }
        


 
        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
            
        }
    
    }
}
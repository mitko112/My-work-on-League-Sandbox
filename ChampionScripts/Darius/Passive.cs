
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace CharScripts
{
     
    public class CharScriptDarius : ICharScript

    {
        Spell Spell;
        int counter;
        AttackableUnit Target;
        
        public void OnActivate(ObjAIBase owner, Spell spell)

        {

            Spell = spell;


            {
                ApiEventManager.OnHitUnit.AddListener(this, owner, OnHitUnit, false);
            }
        }
        public void OnHitUnit(DamageData damageData)

        
        {
            var owner = Spell.CastInfo.Owner;
            var target = damageData.Target;
            AddBuff("DariusHemoMarker", 5f, 1, Spell, target, owner);

        }
        


 
        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }
        public void OnUpdate(float diff)
        {
        }
    }
}

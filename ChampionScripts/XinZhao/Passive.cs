
using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;


namespace CharScripts
{
     
    public class CharScriptXinzhao : ICharScript

    {
        Spell Spell;
        public void OnActivate(ObjAIBase owner, Spell spell)

        {

            Spell = spell;


            {
                ApiEventManager.OnHitUnit.AddListener(this, owner, OnHitUnit, false);
            }
        }
        public void OnHitUnit(DamageData damageData)

        
        {
            var target = damageData.Target;
            var owner = Spell.CastInfo.Owner;
            AddBuff("XenZhaoPuncture", 3f, 1, Spell, target, owner);

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
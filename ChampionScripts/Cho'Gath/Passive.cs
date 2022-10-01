
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace CharScripts
{
     
    public class CharScriptChogath : ICharScript

    {
        Spell Spell;
        public void OnActivate(ObjAIBase owner, Spell spell)

        {

            Spell = spell;


            {
                ApiEventManager.OnKillUnit.AddListener(this,owner , OnKillUnit, false);
            }
        }
        public void OnKillUnit(DeathData deathData)


        {
            var owner = Spell.CastInfo.Owner;
            float heal = 17 + 3 * Spell.CastInfo.SpellLevel;
            float mana = 3.25f + 0.25f* Spell.CastInfo.SpellLevel;
            owner.Stats.CurrentHealth += heal;
            owner.Stats.CurrentMana += mana;
            AddParticleTarget(owner, owner, "Global_Heal.troy", owner, 1f);
            AddParticleTarget(owner, owner, "globalhit_mana.troy", owner, 1f);
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

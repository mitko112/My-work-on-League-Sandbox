using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using GameServerLib.GameObjects.AttackableUnits;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace CharScripts
{
     
    public class CharScriptChogath : ICharScript

    {
        ISpell Spell;
        public void OnActivate(IObjAiBase owner, ISpell spell)

        {

            Spell = spell;


            {
                ApiEventManager.OnKillUnit.AddListener(this,owner , OnKillUnit, false);
            }
        }
        public void OnKillUnit(IDeathData deathData)


        {
            var owner = Spell.CastInfo.Owner;
            float heal = 17 + 3 * Spell.CastInfo.SpellLevel;
            float mana = 3.25f + 0.25f* Spell.CastInfo.SpellLevel;
            owner.Stats.CurrentHealth += heal;
            owner.Stats.CurrentMana += mana;
            AddParticleTarget(owner, owner, "Global_Heal.troy", owner, 1f);
            AddParticleTarget(owner, owner, "globalhit_mana.troy", owner, 1f);
        }
        


 
        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }
        public void OnUpdate(float diff)
        {
        }
    }
}
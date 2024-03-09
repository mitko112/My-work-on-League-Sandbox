
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace CharScripts
{
    public class CharScriptKogmaw : ICharScript
    {
        private Spell Spell;
        private int counter;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnDeath.AddListener(this, owner, OnDeath, true);
            ApiEventManager.OnResurrect.AddListener(this, owner, OnRessurect, false);
            Spell = spell;
        }

        public void OnDeath(DeathData deathData)
        {
            AddBuff("KogmawPassiveDelay", 2f, 1, Spell, deathData.Unit, deathData.Unit as ObjAIBase);
        }

        public void OnRessurect(ObjAIBase owner)
        {
            counter++;
            //This is to avoid a loop in his passive.
            if (counter == 2)
            {
                ApiEventManager.OnDeath.AddListener(this, owner, OnDeath, true);
                counter = 0;
            }
        }

      
    }
}
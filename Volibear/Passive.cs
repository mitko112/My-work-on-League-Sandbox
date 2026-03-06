
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeaguePackets.Game.Common.CastInfo;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace CharScripts
{
    public class CharScriptVolibear : ICharScript
    {
        ObjAIBase Owner;
        float timer = 0;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Owner = owner;
        }

        public void OnUpdate(float diff)
        {
            timer -= diff;
            if (Owner.Stats.CurrentHealth <= Owner.Stats.HealthPoints.Total * 0.3f && timer <= 0)
            {
                AddBuff("VolibearPassiveBuff", 6f, 1, null, Owner, Owner, false);
                timer = 120000f;
            }
        }
    }
}
    


            
       



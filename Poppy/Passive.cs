using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeaguePackets.Game;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeaguePackets.Game.Common.CastInfo;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace CharScripts
{

    public class CharScriptPoppy : ICharScript

    {
        ObjAIBase Owner;
        public void OnActivate(ObjAIBase owner, Spell spell)

        {
            Owner = owner;



            ApiEventManager.OnTakeDamage.AddListener(this, owner, OnTakeDamage, false);
        }
       
        public void OnTakeDamage(DamageData data)
        {
            if (data.Damage > Owner.Stats.CurrentHealth*0.1f)
           {
                data.Damage *= 0.5f; 
            }
        }



        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
            
        }
        public void OnUpdate(float diff)
        {
        }
    }
}

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

    public class CharScriptVayne : ICharScript

    {
        
        public void OnActivate(ObjAIBase owner, Spell spell)

        {
            AddBuff("PassiveMSVayne", 5f, 1, spell, owner, owner, true);

        }


        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {

        }
        public void OnUpdate(float diff)
        {
        }
    }
}
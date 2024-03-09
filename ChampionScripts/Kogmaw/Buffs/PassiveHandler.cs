using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;

namespace Buffs
{
    internal class KogmawPassiveDelay : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_DEHANCER,
            BuffAddType = BuffAddType.RENEW_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; }
        Vector2 xy;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            xy = unit.Position;
            unit.SetIsTargetableToTeam(TeamId.TEAM_BLUE, false);
            unit.SetIsTargetableToTeam(TeamId.TEAM_PURPLE, false);
            unit.SetIsTargetableToTeam(TeamId.TEAM_PURPLE, false);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var champion = unit as Champion;
            champion.Respawn();//Implement a custom spawn position function later
            champion.TeleportTo(xy.X, xy.Y);
            unit.SetIsTargetableToTeam(TeamId.TEAM_BLUE, true);
            unit.SetIsTargetableToTeam(TeamId.TEAM_PURPLE, true);
            unit.SetIsTargetableToTeam(TeamId.TEAM_PURPLE, true);
            AddBuff("KogmawPassive", 60f, 1, ownerSpell, unit, champion);
        }

       
    }
}
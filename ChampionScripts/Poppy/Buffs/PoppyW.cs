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
    internal class PoppyParagonSpeed : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.HASTE,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {

            StatsModifier.MoveSpeed.PercentBonus += 0.06f + 0.04f * ownerSpell.CastInfo.SpellLevel; 
            unit.AddStatModifier(StatsModifier);
            
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
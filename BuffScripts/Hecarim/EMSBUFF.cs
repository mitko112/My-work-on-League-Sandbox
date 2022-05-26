using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class HecarimRamp : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.HASTE,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 5
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner =  ownerSpell.CastInfo.Owner;
            StatsModifier.MoveSpeed.PercentBonus += 0.25f; 
            unit.AddStatModifier(StatsModifier);
            
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
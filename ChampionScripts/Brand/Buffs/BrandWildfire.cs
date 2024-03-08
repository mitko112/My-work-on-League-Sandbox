using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Buffs
{
    internal class BrandWildfire : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_OVERLAPS,
            MaxStacks = 5
            
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

       

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            LogInfo("BrandWildfire OnDeactivate called. Stacks: " + buff.StackCount + " Elapsed: " + buff.Elapsed());
        }
           
        
    }
}

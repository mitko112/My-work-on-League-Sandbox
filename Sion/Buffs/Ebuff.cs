using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;

namespace Buffs
{
    internal class SionEnrage : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        
        public void OnUpdate(float diff)
        {
        }

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            AddParticleTarget(owner, unit, "Enrageweapon_buf.troy", unit);
            StatsModifier.AttackDamage.FlatBonus = 25 + (10 * ownerSpell.CastInfo.SpellLevel);
            unit.AddStatModifier(StatsModifier);
        }

    }
}
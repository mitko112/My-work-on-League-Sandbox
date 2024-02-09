
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;

namespace Buffs
{
    internal class Masochism : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;

            float AD = ((100 / owner.Stats.HealthPoints.Total) * (owner.Stats.HealthPoints.Total - owner.Stats.CurrentHealth) * 0.25f + (0.15f * ownerSpell.CastInfo.SpellLevel));
            float BaseDamage = 25f + (15f * ownerSpell.CastInfo.SpellLevel);
            float damage = BaseDamage + AD;

            StatsModifier.AttackDamage.FlatBonus = damage;
            unit.AddStatModifier(StatsModifier);
            //TODO: Make the AD Buff update based on Lost HP
        }

        
        }
    }

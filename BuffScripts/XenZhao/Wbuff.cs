using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Collections.Generic;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class XenZhaoBattleCryPassive : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 3
        };
        public IStatsModifier StatsModifier { get; private set; }

        IParticle p;
        IAttackableUnit Unit;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Unit = unit;
            switch (buff.StackCount)
            {
                case 1:
                    
                    break;
                case 2:
                    
                    break;
                case 3:
                    AddParticleTarget(ownerSpell.CastInfo.Owner, null, "Global_Heal.troy", unit);
                    Heal(ownerSpell);
                    buff.DeactivateBuff();
                    break;
            }
        }
        public void Heal(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            float heal = 26*(owner.GetSpell(1).CastInfo.SpellLevel - 1);
            float ap = owner.Stats.AbilityPower.Total * 0.7f;
            owner.Stats.CurrentHealth += heal+ap;
            
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
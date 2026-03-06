using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;
using GameServerLib.GameObjects.AttackableUnits;
using System.Numerics;


namespace Buffs
{
    internal class XenZhaoBattleCryPassive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 3
        };
        public StatsModifier StatsModifier { get; private set; }

        Particle p;
        AttackableUnit Unit;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
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
        public void Heal(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            float heal = 26*(owner.GetSpell("XenZhaoBattleCry").CastInfo.SpellLevel - 1);
            float ap = owner.Stats.AbilityPower.Total * 0.7f;
            owner.Stats.CurrentHealth += heal+ap;
            
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
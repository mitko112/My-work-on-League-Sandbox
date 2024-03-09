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
    internal class PoppyParagonManager : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 10,
            IsHidden = false
        };
        

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private float timeSinceLastTick;
        private AttackableUnit Unit;
        private float TickingDamage;
        private ObjAIBase Owner;
        private Spell spell;
        private bool limiter = false;
        float stackdamage;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            Owner = owner;
            var stackdamage1 = (1f + ownerSpell.CastInfo.SpellLevel * 0.5f) * owner.GetBuffWithName("PoppyParagonManager").StackCount;
            stackdamage = stackdamage1;
            owner.Stats.AttackDamage.FlatBonus = stackdamage;
            owner.Stats.Armor.FlatBonus = stackdamage;
        }



        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Owner.Stats.AttackDamage.FlatBonus -= stackdamage;
            Owner.Stats.Armor.FlatBonus -= stackdamage;
            //Owner.RemoveBuffsWithName("PoppyParagonManager");
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
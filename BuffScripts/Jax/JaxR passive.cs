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
    internal class JaxRelentlessAttack : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 3
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

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
                    AddParticleTarget(ownerSpell.CastInfo.Owner, null, "RelentlessAssault_tar.troy", unit, 1f);
                    TargetExecute(ownerSpell);
                    buff.DeactivateBuff();
                    break;



            }
        }
        public void TargetExecute(Spell spell)


        {

            var owner = spell.CastInfo.Owner;
            var AP = owner.Stats.AbilityPower.Total * 0.7f;

            float damage = 100 * owner.GetSpell("JaxRelentlessAssault").CastInfo.SpellLevel + AP;



            Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);




        }


    }
}

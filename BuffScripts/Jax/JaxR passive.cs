using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace Buffs
{
    internal class JaxRelentlessAttack : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 3
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

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
                    AddParticleTarget(ownerSpell.CastInfo.Owner, null, "RelentlessAssault_tar.troy", unit, 1f);
                    TargetExecute(ownerSpell);
                    buff.DeactivateBuff();
                    break;
           
               

            }
        }
        public void TargetExecute(ISpell spell)


        {

            var owner = spell.CastInfo.Owner;
            var AP = owner.Stats.AbilityPower.Total * 0.7f;
            
            float damage = 100 * owner.GetSpell(1).CastInfo.SpellLevel + AP;

           

                Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            



        }



        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            

            

        }

        public void OnUpdate(float diff)
        {

        }
    }
}
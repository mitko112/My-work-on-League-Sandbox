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
    internal class AkaliMotaImpact : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        IBuff thisBuff;


        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {

            thisBuff = buff;





            if (unit is IObjAiBase obj)
            {
                ApiEventManager.OnLaunchAttack.AddListener(this, obj, TargetExecute, true);




            }

        }
        public void TargetExecute(ISpell spell)
        {




            var owner = spell.CastInfo.Owner;
            float ap = owner.Stats.AbilityPower.Total * 0.5f;
            var target = spell.CastInfo.Targets[0].Unit;
            float damage = 45 * owner.GetSpell(0).CastInfo.SpellLevel + ap;

            if (target.HasBuff("AkaliMota"))


            {

                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(owner, target, "akali_mark_impact_tar.troy", target, 1f);
                RemoveBuff(target, "AkaliMota");

                

            }

        }


        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {

            thisBuff.DeactivateBuff();
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
        }


        public void OnUpdate(float diff)
        {


        }
    }
}


    

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
    internal class AkaliMotaImpact : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        Buff thisBuff;


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {

            thisBuff = buff;





            if (unit is ObjAIBase obj)
            {
                ApiEventManager.OnLaunchAttack.AddListener(this, obj, TargetExecute, true);




            }

        }
        public void TargetExecute(Spell spell)
        {




            var owner = spell.CastInfo.Owner;
            float ap = owner.Stats.AbilityPower.Total * 0.5f;
            var target = spell.CastInfo.Targets[0].Unit;
            float damage = 45 + 25 * (owner.GetSpell("AkaliMota").CastInfo.SpellLevel - 1) + ap;

            if (target.HasBuff("AkaliMota"))


            {

                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(owner, target, "akali_mark_impact_tar.troy", target, 1f);
                RemoveBuff(target, "AkaliMota");

                

            }

        }


        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {

            thisBuff.DeactivateBuff();
            ApiEventManager.OnLaunchAttack.RemoveListener(this);
        }


        public void OnUpdate(float diff)
        {


        }
    }
}




    

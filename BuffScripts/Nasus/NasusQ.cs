
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
using LeagueSandbox.GameServer.GameObjects.Other;

namespace Buffs
{

    internal class NasusQ : IBuffGameScript
    {

        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Buff thisBuff;
        ObjAIBase Unit;
        Spell Spell;
        

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            if (unit is ObjAIBase ai)
            {
                Unit = ai;

                ApiEventManager.OnHitUnit.AddListener(this, ai, TargetExecute, true);

                ai.SkipNextAutoAttack();


            }

        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Spell = ownerSpell;

        }
        public void TargetExecute(DamageData damageData)
        {
            if (!thisBuff.Elapsed() && thisBuff != null && Unit != null)
            {

                //float stacks = Unit.GetBuffWithName("NasusQStacks").StackCount;
                float damage = 30; //+ stacks;
                
                var target = damageData.Target;
                target.TakeDamage(Unit, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                
                if (target.IsDead)
                {
                    AddBuff("NasusQStacks", 1, 1, Spell, Unit, Unit, true);
                    
                }

                thisBuff.DeactivateBuff();
            }
        }
        public void OnUpdate(float diff)
        {

        }
    }
}

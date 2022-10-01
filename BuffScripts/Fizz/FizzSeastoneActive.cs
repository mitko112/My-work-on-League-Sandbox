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
    internal class FizzSeastoneActive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {

            if (unit is ObjAIBase obj)
            {
                ApiEventManager.OnLaunchAttack.AddListener(this, obj, TargetExecute, false);




            }
        }
        public void TargetExecute(Spell spell)


        {

            var owner = spell.CastInfo.Owner;
            var target = spell.CastInfo.Targets[0].Unit;
            var AP = owner.Stats.AbilityPower.Total;
            float damage = 10 * owner.GetSpell("FizzSeastonePassive").CastInfo.SpellLevel + AP;
            AddParticleTarget(owner, owner, "Fizz_SeastoneTrident.troy", owner, 5f, bone: "BUFFBONE_GLB_WEAPON_1");
            AddParticleTarget(owner, owner, "Fizz_SeastonePassive_Weapon.troy", owner, bone: "BUFFBONE_GLB_WEAPON_1");


            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);


        }



        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);

        }

        public void OnUpdate(float diff)
        {

        }
    }
}

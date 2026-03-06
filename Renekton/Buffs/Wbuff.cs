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
    internal class RenektonExecute : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        Buff thisBuff;
        ObjAIBase Unit;
        Particle bladeParticle;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;

            if (unit is ObjAIBase ai)
            {
                Unit = ai;

                ApiEventManager.OnHitUnit.AddListener(this, ai, TargetExecute, true);

                // Prevent default AA, we handle visuals
                ai.SkipNextAutoAttack();

                // Weapon glow
                bladeParticle = AddParticleTarget(
    Unit,
    Unit,
    "Renekton_Weapon_Hot.troy",
    Unit,
    bone: "BUFFBONE_GLB_HAND_R"
);
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(bladeParticle);
        }

        public void TargetExecute(DamageData damageData)
        {
            if (thisBuff == null || Unit == null || thisBuff.Elapsed())
                return;

            var target = damageData.Target;
            if (target == null || target.IsDead)
                return;

            int spellLevel = Unit.GetSpell("RenektonPreExecute").CastInfo.SpellLevel;

            float bonusDamage =
                10 * spellLevel +
                Unit.Stats.AttackDamage.Total * 1.5f;

            var mana = 10;
            Unit.Stats.CurrentMana += mana;
            // REAL auto already happened (engine)

            // Fake second strike animation
            PlayAnimation(Unit, "Spell2", 1.0f);

            // Second (animated) damage instance
            target.TakeDamage(
                Unit,
                bonusDamage,
                DamageType.DAMAGE_TYPE_PHYSICAL,
                DamageSource.DAMAGE_SOURCE_SPELL,
                false

            );

            thisBuff.DeactivateBuff();
        }



    }
}
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class HecarimRampAttack : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private ObjAIBase Unit;
        private Buff ThisBuff;

        private Vector2 startPosition;

        // ===== RETAIL VALUES =====
        private const float MAX_DISTANCE = 600f;

        private readonly float[] MIN_DAMAGE = { 40f, 55f, 70f, 85f, 100f };
        private readonly float[] MAX_DAMAGE = { 80f, 110f, 140f, 170f, 200f };

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ThisBuff = buff;

            if (unit is ObjAIBase ai)
            {
                Unit = ai;

                // Store start position safely (LS uses X/Y)
                startPosition = new Vector2(ai.Position.X, ai.Position.Y);

                ApiEventManager.OnHitUnit.AddListener(this, ai, TargetExecute, true);

                // Prevent double AA visuals
                ai.SkipNextAutoAttack();
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }

        private float GetTraveledDistance()
        {
            Vector2 current = new Vector2(Unit.Position.X, Unit.Position.Y);
            return Vector2.Distance(startPosition, current);
        }

        private float GetBonusDamage()
        {
            int rank = Unit.GetSpell("HecarimRamp").CastInfo.SpellLevel - 1;
            if (rank < 0) rank = 0;

            float min = MIN_DAMAGE[rank];
            float max = MAX_DAMAGE[rank];

            float distance = GetTraveledDistance();
            float ratio = Math.Clamp(distance / MAX_DISTANCE, 0f, 1f);

            return min + (max - min) * ratio;
        }

        public void TargetExecute(DamageData damageData)
        {
            if (ThisBuff == null || ThisBuff.Elapsed() || Unit == null)
                return;

            var target = damageData.Target;

            // BONUS DAMAGE ONLY (auto attack already applied)
            float bonusDamage = GetBonusDamage();

            target.TakeDamage(
                Unit,
                bonusDamage,
                DamageType.DAMAGE_TYPE_PHYSICAL,
                DamageSource.DAMAGE_SOURCE_SPELL,
                false
            );

            // Knockback
            Vector2 knockbackPos = GetPointFromUnit(Unit, 450f);
            ForceMovement(target, "RUN", knockbackPos, 2200, 0, 0, 0);

            ThisBuff.DeactivateBuff();
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
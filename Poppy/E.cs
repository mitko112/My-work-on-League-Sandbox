using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class PoppyHeroicCharge : ISpellScript
    {
        private ObjAIBase owner;
        private AttackableUnit target;
        private Spell spell;

        private const float PoppyDashSpeed = 2400f;
        private const float PushSpeed = 1800f;

        private const float PoppyStopDistance = 65f;
        private const float CarryDistance = 475f;
        private const float FollowOffset = 65f;

        private const float WallCheckRadius = 35f;
        private const float Step = 25f;

        public SpellScriptMetadata ScriptMetadata { get; } = new()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnSpellPreCast(
            ObjAIBase owner,
            Spell spell,
            AttackableUnit target,
            Vector2 start,
            Vector2 end)
        {
            this.owner = owner;
            this.spell = spell;
            this.target = target;
        }

        public void OnSpellPostCast(Spell spell)
        {
            if (owner == null || target == null || target.IsDead)
                return;

            int level = spell.CastInfo.SpellLevel;

            /* Initial damage */
            float damage = (50f + 25f * level) + owner.Stats.AbilityPower.Total * 0.4f;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL,
                DamageSource.DAMAGE_SOURCE_SPELL, false);

            /* Initial direction */
            Vector2 dir = Vector2.Normalize(target.Position - owner.Position);
            if (dir == Vector2.Zero)
                dir = Vector2.UnitX;

            /* Dash Poppy to target */
            Vector2 poppyDashEnd = target.Position - dir * PoppyStopDistance;
            float dashTime = Vector2.Distance(owner.Position, poppyDashEnd) / PoppyDashSpeed;

            ForceMovement(owner, "Spell4", poppyDashEnd, PoppyDashSpeed, 0, 0, 0);

            /* After dash */
            CreateTimer(dashTime, () =>
            {
                if (target == null || target.IsDead || owner == null)
                    return;

                owner.SetPosition(poppyDashEnd);

                /* Recompute direction */
                Vector2 newDir = Vector2.Normalize(target.Position - owner.Position);
                if (newDir == Vector2.Zero)
                    newDir = Vector2.UnitX;

                bool hitWall = TryGetWallPoint(
                    target.Position,
                    newDir,
                    CarryDistance,
                    out Vector2 targetEnd
                );

                /* Push target */
                ForceMovement(target, "Spell3", targetEnd, PushSpeed, 0, 0, 0);

                if (hitWall)
                {
                    ApplyWallHit(target);
                }
                else
                {
                    /* 🔥 FOLLOW TARGET (KEY FIX) */
                    Vector2 poppyFollowEnd = targetEnd - newDir * FollowOffset;
                    ForceMovement(owner, "Spell4", poppyFollowEnd, PushSpeed, 0, 0, 0);
                }
            });
        }

        private void ApplyWallHit(AttackableUnit unit)
        {
            int level = spell.CastInfo.SpellLevel;

            float bonusDamage = (75f + 75f * level) +
                                owner.Stats.AbilityPower.Total * 0.4f;

            unit.TakeDamage(owner, bonusDamage,
                DamageType.DAMAGE_TYPE_MAGICAL,
                DamageSource.DAMAGE_SOURCE_SPELL, false);

            AddBuff("Stun", 1.5f, 1, spell, unit, owner);
        }

        private bool TryGetWallPoint(Vector2 start, Vector2 dir, float maxDist, out Vector2 result)
        {
            for (float d = 0f; d <= maxDist; d += Step)
            {
                Vector2 p = start + dir * d;
                if (!IsWalkable(p.X, p.Y, WallCheckRadius))
                {
                    result = p - dir * Step;
                    return true;
                }
            }

            result = start + dir * maxDist;
            return false;
        }
    }
}
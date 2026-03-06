using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using System.Numerics;
using GameServerLib.GameObjects.AttackableUnits;

namespace Spells

{

    public class VayneCondemn : ISpellScript
    {


        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,

            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            SpellCast(owner, 1, SpellSlotType.ExtraSlots, false, target, Vector2.Zero);

        }
    }
}

namespace Spells
{
    public class VayneCondemnMissile : ISpellScript
    {
        private ObjAIBase Owner;
        private Spell Spell;

        private const float PushSpeed = 2200f;
        private const float PushDistance = 470f;
        private const float WallCheckRadius = 35f;
        private const float Step = 25f;

        public SpellScriptMetadata ScriptMetadata { get; } = new()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,

            // 🔥 THIS IS THE REAL FIX
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            }
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Owner = owner;
            Spell = spell;

            ApiEventManager.OnSpellHit.AddListener(
                this,
                spell,
                OnSpellHit,
                false
            );
        }

        public void OnSpellHit(
            Spell spell,
            AttackableUnit target,
            SpellMissile missile,
            SpellSector sector)
        {
            if (target == null || target.IsDead)
                return;

            FaceDirection(Owner.Position, target);

            int level = spell.CastInfo.SpellLevel;

            float damage =
                45f * level +
                Owner.Stats.AttackDamage.FlatBonus * 0.5f;

            target.TakeDamage(
                Owner,
                damage,
                DamageType.DAMAGE_TYPE_PHYSICAL,
                DamageSource.DAMAGE_SOURCE_SPELL,
                false
            );
            AddBuff("VayneSilveredBolts", 3f, 1, spell, target, Owner);
            Vector2 dir = Vector2.Normalize(target.Position - Owner.Position);
            if (dir == Vector2.Zero)
                dir = Vector2.UnitX;

            bool hitWall = TryGetWallPoint(
                target.Position,
                dir,
                PushDistance,
                out Vector2 targetEnd
            );

            ForceMovement(
                target,
                "RUN",
                targetEnd,
                PushSpeed,
                0,
                0,
                0
            );

            if (hitWall)
            {
                AddBuff("Stun", 1.5f, 1, spell, target, Owner);
            }

            missile.SetToRemove();
        }

        private bool TryGetWallPoint(
            Vector2 start,
            Vector2 dir,
            float maxDist,
            out Vector2 result)
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
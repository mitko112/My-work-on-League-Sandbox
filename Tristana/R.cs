using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using GameServerCore.Enums;

namespace Spells
{
    public class BusterShot : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } =
            new SpellScriptMetadata
            {
                TriggersSpellCasts = true,
                IsDamagingSpell = true,
                MissileParameters = new MissileParameters
                {
                    Type = MissileType.Target
                }
            };

        ObjAIBase Owner;
        Spell Spell;

        const float AOE_RADIUS = 300f;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Owner = owner;
            Spell = spell;

            ApiEventManager.OnSpellHit.AddListener(this, spell, OnSpellHit, false);
        }

        public void OnSpellHit(
            Spell spell,
            AttackableUnit primaryTarget,
            SpellMissile missile,
            SpellSector sector)
        {
            FaceDirection(Owner.Position, primaryTarget);

            float damage =
                200 +
                100 * Spell.CastInfo.SpellLevel +
                Owner.Stats.AbilityPower.Total * 1.5f;

            var units = GetUnitsInRange(
    primaryTarget.Position,
    AOE_RADIUS,
    true
);

            foreach (var unit in units)
            {
                if (!(unit is AttackableUnit target))
                    continue;

                if (target.Team == Owner.Team || target.IsDead)
                    continue;

                // Damage
                target.TakeDamage(
                    Owner,
                    damage,
                    DamageType.DAMAGE_TYPE_MAGICAL,
                    DamageSource.DAMAGE_SOURCE_SPELL,
                    false
                );

                // Knockback
                Vector2 knockbackPoint =
                    GetPointFromUnit(
                        target,
                        -(400 + 200 * Spell.CastInfo.SpellLevel));

                ForceMovement(
                    target,
                    "RUN",
                    knockbackPoint,
                    2200,
                    0,
                    0,
                    0
                );

                AddParticleTarget(
                    Owner,
                    target,
                    "tristana_bustershot_tar",
                    target,
                    1f,
                    1f
                );
            }

            AddParticleTarget(
                Owner,
                Owner,
                "BusterShot_cas.troy",
                Owner,
                1f,
                1f
            );

            missile.SetToRemove();
        }
    }
}
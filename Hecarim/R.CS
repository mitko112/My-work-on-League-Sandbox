using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class HecarimUlt : ISpellScript
    {
        private ObjAIBase Owner;

        public SpellScriptMetadata ScriptMetadata { get; private set; } =
            new SpellScriptMetadata
            {
                MissileParameters = new MissileParameters
                {
                    Type = MissileType.Circle
                },
                IsDamagingSpell = true
            };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Owner = owner;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnSpellPreCast(
            ObjAIBase owner,
            Spell spell,
            AttackableUnit target,
            Vector2 start,
            Vector2 end
        )
        {
            FaceDirection(end, owner);

            // ✅ Hecarim dash (retail behavior)
            Vector2 dashEnd = GetPointFromUnit(owner, 750f);
            ForceMovement(owner, "Spell3", dashEnd, 1200, 0, 0, 0);
        }

        public void TargetExecute(
            Spell spell,
            AttackableUnit target,
            SpellMissile missile,
            SpellSector sector
        )
        {
            var owner = spell.CastInfo.Owner;
            var ap = owner.Stats.AbilityPower.Total;
            var damage = 150 * spell.CastInfo.SpellLevel + ap;

            AddParticle(owner, null, "Hecarim_R.troy", owner.Position, direction: owner.Direction);

            var units = GetUnitsInRange(owner.Position, 750f, true);

            for (int i = 0; i < units.Count; i++)
            {
                var unit = units[i];

                if (unit.Team == owner.Team ||
                    unit is BaseTurret ||
                    unit is ObjBuilding ||
                    unit is Inhibitor)
                    continue;

                // Damage
                unit.TakeDamage(
                    owner,
                    damage,
                    DamageType.DAMAGE_TYPE_MAGICAL,
                    DamageSource.DAMAGE_SOURCE_SPELLAOE,
                    false
                );

                // ✅ Apply FEAR ONLY
                // No ForceMovement here — LS AI handles flee correctly
                AddBuff(
     "Flee",
     1.25f,
     1,
     spell,
     unit,
     owner
 );

                // 🔧 CRITICAL FIX:
                // Re-issue flee order after dash finishes
                CreateTimer(0.1f, () =>
                {
                    if (!unit.IsDead && unit.HasBuff("Flee"))
                    {
                        AddBuff(
                            "Flee",
                            1.0f, // remaining duration doesn't matter
                            1,
                            spell,
                            unit,
                            owner
                        );
                    }
                });
            }

            missile.SetToRemove();
        }
    }
}
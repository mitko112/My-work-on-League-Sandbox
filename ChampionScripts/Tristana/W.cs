using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using System.Collections.Generic;

namespace Spells
{
    public class RocketJump : ISpellScript
    {
        public  SpellScriptMetadata ScriptMetadata { get; } = new()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true
        };

        Spell Spell;
        public  void OnSpellPostCast(Spell spell)
        {
            Spell = spell;
            var owner = spell.CastInfo.Owner;
            PlayAnimation(owner, "spell2");
            SetStatus(owner, StatusFlags.Ghosted, true);
            var SpellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            float Dist = Vector2.Distance(owner.Position, SpellPos);
            if (Dist > spell.SpellData.CastRangeDisplayOverride)
            {
                Dist = spell.SpellData.CastRangeDisplayOverride;
                SpellPos = GetPointFromUnit(owner, spell.SpellData.CastRangeDisplayOverride);
            }
            FaceDirection(SpellPos, owner, true);
            ForceMovement(owner, null, SpellPos, Dist * 1.22f, 0, Dist * 0.05f, 0);
            AddParticlePos(owner, "tristana_rocketJump_cas", owner.Position, owner.Position);
            AddParticleTarget(owner, owner, "tristana_rocketJump_cas_sparks", owner); 
            ApiEventManager.OnMoveEnd.AddListener(this, owner, OnMoveEnd, true);
            ApiEventManager.OnMoveSuccess.AddListener(this, owner, OnMoveSuccess, true);
        }
        public void OnMoveSuccess(AttackableUnit unit)
        {

            var owner = Spell.CastInfo.Owner; 
            owner.SetDashingState(false);
            SetStatus(owner, StatusFlags.Ghosted, false);
            PlayAnimation(owner, "spell3_landing", 0.2f);
            StopAnimation(owner, "spell3", true, true, true);
            AddParticlePos(owner, "tristana_rocketJump_land", owner.Position, owner.Position);
            float Damage = 25 + 45 * Spell.CastInfo.SpellLevel + owner.Stats.AbilityPower.Total * 0.8f;
            List<AttackableUnit> Units = GetUnitsInRange(owner.Position, 270, true);
            Units.RemoveAll(x => x is BaseTurret || x is ObjBuilding);
            foreach (AttackableUnit Unit in Units)
            {
                AddParticleTarget(owner, Unit, "tristana_rocketJump_unit_tar",Unit, 1f,  1f);
                AddBuff("RocketJumpSlow", 0.5f * (1 + Spell.CastInfo.SpellLevel), 1, Spell, Unit, owner, false);
                Unit.TakeDamage(owner, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            }
        }
        public void OnMoveEnd(AttackableUnit owner)
        {
            SetStatus(owner, StatusFlags.Ghosted, false);
            StopAnimation(owner, "spell3", true, true, true);
        }
    }
}

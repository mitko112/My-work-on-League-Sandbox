using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeaguePackets.Game.Common.CastInfo;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class FioraQ : ISpellScript
    {
        private ObjAIBase Fiora;
        private Spell FioraDash;
        private AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            FioraDash = spell;
            Fiora = owner = spell.CastInfo.Owner as Champion;
        }
        public void OnSpellPostCast(Spell spell)
        {
            Fiora.CancelAutoAttack(true);
            if (!Fiora.HasBuff("FioraQCD")) { AddBuff("FioraQCD", 4, 1, spell, Fiora, Fiora); }
            SpellCast(Fiora, 0, SpellSlotType.ExtraSlots, false, Target, Vector2.Zero);
        }
    }

    public class FioraQLunge : ISpellScript
    {
        float Dist;
        float Damage;
        Vector2 TargetPos;
        private ObjAIBase Fiora;
        private Spell FioraDash;
        private AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            IsDamagingSpell = true,
            TriggersSpellCasts = true
        };
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            FioraDash = spell;
            Fiora = owner = spell.CastInfo.Owner as Champion;
            Fiora.SetTargetUnit(null, true);
            SetStatus(Fiora, StatusFlags.Ghosted, true);
            Fiora.CancelAutoAttack(true);
        }
        public void OnSpellCast(Spell spell)
        {
            ApiEventManager.OnMoveEnd.AddListener(this, Fiora, OnMoveEnd, true);
            ApiEventManager.OnMoveSuccess.AddListener(this, Fiora, OnMoveSuccess, true);
            Dist = System.Math.Abs(Vector2.Distance(Target.Position, Fiora.Position)) - 125;
            TargetPos = GetPointFromUnit(Fiora, Dist);
            PlayAnimation(Fiora, "Spell1");
            FaceDirection(TargetPos, Fiora, true);
            AddParticleTarget(Fiora, Fiora, "Fiora_Dance_windup.troy", Fiora);
            AddParticleTarget(Fiora, Fiora, "FioraQLunge_dashtrail.troy", Fiora);
            ForceMovement(Fiora, null, TargetPos, 2200, 0, 0, 0, movementOrdersType: ForceMovementOrdersType.CANCEL_ORDER);
        }
        public void OnMoveSuccess(AttackableUnit unit)
        {
            Fiora.SetDashingState(false);
            Damage = 15 + (25f * Fiora.Spells[0].CastInfo.SpellLevel) + (Fiora.Stats.AttackDamage.FlatBonus * 1.2f);
            Target.TakeDamage(Fiora, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddParticleTarget(Fiora, Target, "FioraQLunge_tar", Target);
            if (Fiora.Team != Target.Team && Target is Champion)
            {
                Fiora.SetTargetUnit(Target, true);
                Fiora.UpdateMoveOrder(OrderType.AttackTo, true);
            }
        }
        public void OnMoveEnd(AttackableUnit owner)
        {
            Fiora.SetDashingState(false);
            SetStatus(Fiora, StatusFlags.Ghosted, false);
            StopAnimation(Fiora, "spell1", true, true, true);
        }
    }
}

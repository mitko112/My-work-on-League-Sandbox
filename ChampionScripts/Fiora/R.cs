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
using System.Collections.Generic;
using System.Numerics;
using static LeaguePackets.Game.Common.CastInfo;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class FioraDance : ISpellScript
    {
        ObjAIBase Fiora;
        Vector2 TargetPos;
        AttackableUnit Target;
        public List<AttackableUnit> UnitsHit = Spells.FioraDanceStrike.UnitsHit;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnSpellPostCast(Spell spell)
        {
            UnitsHit.Clear();
            Target = spell.CastInfo.Targets[0].Unit;
            Fiora = spell.CastInfo.Owner as Champion;
            Fiora.SetTargetUnit(null, true);
            AddBuff("FioraDance", 2.25f, 1, spell, Fiora, Fiora);
            //ForceMovement(Fiora, null, new Vector2(Fiora.Position.X + 8f, Fiora.Position.Y + 8f), 13f, 0, 16.5f, 0);
            //TargetPos = GetPointFromUnit(Fiora,Abs(Vector2.Distance(Target.Position, Fiora.Position)) + 250); 
            //TeleportTo(Fiora, TargetPos.X, TargetPos.Y);  	
        }
    }
    public class FioraDanceStrike : ISpellScript
    {
        float Damage;
        Spell Dance;
        Particle Trail;
        Buff DanceBuff;
        ObjAIBase Fiora;
        Vector2 TargetPos;
        AttackableUnit Target;
        public static List<AttackableUnit> UnitsHit = new List<AttackableUnit>();
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Dance = spell;
            Target = target;
            Fiora = owner = spell.CastInfo.Owner as Champion;
            AddParticleTarget(Fiora, Fiora, "Fiora_Dance_cas", Fiora);
            AddParticleTarget(Fiora, Fiora, "Fiora_Dance_windup", Fiora);
            ForceMovement(Fiora, null, new Vector2(Fiora.Position.X + 40f, Fiora.Position.Y + 40f), 110f, 0, 150f, 0);
            TargetPos = GetPointFromUnit(Fiora, System.Math.Abs(Vector2.Distance(Target.Position, Fiora.Position)) + 175);

        }
        public void OnSpellPostCast(Spell spell)
        {
            Fiora.SetDashingState(false);
            PlayAnimation(Fiora, "spell4c", 0.3f);
            TeleportTo(Fiora, TargetPos.X, TargetPos.Y);
            AddParticleTarget(Fiora, Target, "Fiora_Dance_tar", Target);
            if (!UnitsHit.Contains(Target))
            {
                UnitsHit.Add(Target);
                Damage = (Fiora.Stats.AttackDamage.FlatBonus * 1.2f) + (130f * Fiora.Spells[3].CastInfo.SpellLevel) - 5;
            }
            else
            {
                Damage = ((Fiora.Stats.AttackDamage.FlatBonus * 1.2f) + (130f * Fiora.Spells[3].CastInfo.SpellLevel) - 5) * 0.25f;
            }
            Target.TakeDamage(Fiora, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }
    }
}
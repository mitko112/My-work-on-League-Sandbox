using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class CassiopeiaPetrifyingGaze : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
            
        }


        int StackCount;

        public void OnSpellCast(Spell spell)
        {

            var owner = spell.CastInfo.Owner;
            AddBuff("CassiopeiaPassiveMana", 5f, 1, spell, owner, owner, false);
            StackCount = owner.GetBuffWithName("CassiopeiaPassiveMana").StackCount;
            
            var mana = 10 * StackCount;
            owner.Stats.CurrentMana += mana;

        }
        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;

            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            FaceDirection(spellPos, owner, false);

            var sector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 850f,
                SingleTick = true,
                ConeAngle = 80.00f,
                Type = SectorType.Cone
            });

            
            AddParticleTarget(owner, owner, "CassPetrifyMiss_tar.troy", owner);
            AddParticleTarget(owner, owner, "CassPetrify_cas.troy", owner, 1f, 1f);
        }

        private bool IsFacingCassiopeia(AttackableUnit target, AttackableUnit cassio)
        {
            // Rotation stored in Position.Y (radians)
            float rot = target.Position.Y;

            // Target forward vector
            float forwardX = (float)Math.Cos(rot);
            float forwardY = (float)Math.Sin(rot);

            // Direction from target → Cassio
            float dirX = cassio.Position.X - target.Position.X;
            float dirY = cassio.Position.Y - target.Position.Y;

            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            if (length <= 0.001f)
                return false;

            dirX /= length;
            dirY /= length;

            // Dot product
            float dot = (forwardX * dirX) + (forwardY * dirY);

            // Facing Cassio
            return dot > 0f;
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;

            var ap = owner.Stats.AbilityPower.Total * 0.6f;
            var damage = 200+ spell.CastInfo.SpellLevel + ap;



            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

            if (IsFacingCassiopeia(target, owner))
            {
                // Facing Cass → STUN
                AddBuff("Stun", 2.0f, 1, spell, target, owner);
            }
            else
            {
                // Not facing Cass → SLOW
                AddBuff("CassiopeiaPetrifySlow", 2.0f, 1, spell, target, owner);
            }
        }

        
    }
}




using System.Numerics;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class InfectedCleaverMissileCast : ISpellScript
    {
        ObjAIBase Owner;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Owner = owner;
            //AddBuff("DrMundoPassive", 1, 1, spell, owner, owner, true);
        }

       
        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var targetPos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            float SelfDamage = 40 + 10f * spell.CastInfo.SpellLevel;

            FaceDirection(targetPos, owner);
            SpellCast(owner, 0, SpellSlotType.ExtraSlots, targetPos, targetPos, false, Vector2.Zero);
            owner.StopMovement();
            if (owner.Stats.CurrentHealth > SelfDamage)
            {
                owner.Stats.CurrentHealth -= SelfDamage;
            }
            else
            {
                owner.Stats.CurrentHealth = 1f;
            }
        }
    }

    public class InfectedCleaverMissile : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true
            // TODO
        };

        //Vector2 direction;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

       

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("InfectedCleaverMissileCast").CastInfo.SpellLevel;
            var damage = target.Stats.CurrentHealth * (0.12f + 0.03f * spellLevel);
            float minimunDamage = 30f + (50f * spellLevel);
            float maxDamageMonsters = 200 + 100f * spellLevel;
            float Heal = 20 + 5f * spellLevel;
            //TODO: Implement max damage when monsters gets added.

            if (damage < minimunDamage)
            {
                damage = minimunDamage;
            }

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            owner.Stats.CurrentHealth += Heal;
            AddParticleTarget(owner, null, "dr_mundo_as_mundo_infected_cleaver_tar.troy", target);
            AddParticleTarget(owner, null, "dr_mundo_infected_cleaver_tar.troy", target);
            AddBuff("InfectedCleaverMissile", 2f, 1, spell, target, owner);

            missile.SetToRemove();
        }

        
    }
}

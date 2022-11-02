
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class SionQ: ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            }
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var APratio = owner.Stats.AbilityPower.Total*0.9f;
            var damage = 70*spell.CastInfo.SpellLevel + APratio;

            AddParticleTarget(owner, target, "CrypticGaze_tar.troybin", target, 1f);
            AddParticleTarget(owner, owner, "CrypticGaze_tar.troybin", owner, 1f);
            AddParticleTarget(owner, target, "CrypticGaze_mis.troybin", target, 1f);
            AddBuff("Stun", 1.5f, 1, spell, target, owner);
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
        }

        
        }
    }

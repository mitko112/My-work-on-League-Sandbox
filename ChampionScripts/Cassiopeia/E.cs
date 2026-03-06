using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using System.Security.Principal;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class CassiopeiaTwinFang : ISpellScript
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
        int StackCount;


        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
            
        }

        public void OnSpellCast(Spell spell)
        {

            var owner = spell.CastInfo.Owner;
            AddBuff("CassiopeiaPassiveMana", 5f, 1, spell, owner, owner, false);
            StackCount = owner.GetBuffWithName("CassiopeiaPassiveMana").StackCount;
            
            var mana = 5 + 1 * spell.CastInfo.SpellLevel * StackCount;
            owner.Stats.CurrentMana += mana;

        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var APratio = owner.Stats.AbilityPower.Total*0.55f;
            var damage = 50 + spell.CastInfo.SpellLevel  + APratio;

            if (target.HasBuff("Poisoned"))
            {
                spell.SetCooldown(0.5f, false);
            }

                AddParticleTarget(owner, target, "CassTwinFang_tar.troy", target, 1f);
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}

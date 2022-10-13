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
    public class NasusQ : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, true);
        }
        public void OnLevelUp(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
        }
        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("NasusQ", 6.0f, 1, spell, owner, owner);
        }


        public class NasusQAttack : ISpellScript
        {
            AttackableUnit Target;
            public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
            {
                TriggersSpellCasts = true,
                NotSingleTargetSpell = true,
                IsDamagingSpell = true,
                // TODO
            };

            public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
            {
                Target = target;
            }
            public void OnSpellCast(Spell spell)
            {
                var Owner = spell.CastInfo.Owner;
                if (Owner.HasBuff("NasusQStacks"))
                {
                    float StackDamage = Owner.GetBuffWithName("NasusQStacks").StackCount;
                    float ownerdamage = spell.CastInfo.Owner.Stats.AttackDamage.Total;
                    float damage = 15 + 25 * Owner.GetSpell("NasusQ").CastInfo.SpellLevel + StackDamage + ownerdamage;
                    Target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                    AddParticleTarget(Owner, Target, "Nasus_Base_Q_Tar.troy", Target);
                    if (Target.IsDead)
                    {
                        AddBuff("NasusQStacks", 2500000f, 3, spell, Owner, Owner);
                    }

                }
                else
                {
                    float ownerdamage = spell.CastInfo.Owner.Stats.AttackDamage.Total;
                    float damage = 15 + 25 * Owner.GetSpell("NasusQ").CastInfo.SpellLevel + ownerdamage;
                    Target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                    AddParticleTarget(Owner, Target, "Nasus_Base_Q_Tar.troy", Target);
                    if (Target.IsDead)
                    {
                        AddBuff("NasusQStacks", 2500000f, 3, spell, Owner, Owner);
                    }
                }
            }
        }
    }
}

using System.Collections.Generic;
using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
    {
        public class SadMummyBandageToss : ISpellScript
        {
            IObjAiBase Owner;
            public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
            {
                MissileParameters = new MissileParameters
                {
                    Type = MissileType.Circle
                },
                IsDamagingSpell = true
            };

            public void OnActivate(IObjAiBase owner, ISpell spell)
            {
                ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
            }

            public void OnDeactivate(IObjAiBase owner, ISpell spell)
            {
            }

            public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
            {
                
            }

            public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
            {
                var owner = spell.CastInfo.Owner;
                var APratio = owner.Stats.AbilityPower.Total * 0.7f;
                var damage = 80*(spell.CastInfo.SpellLevel - 0)  + APratio;
                
                
                


                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

                AddParticleTarget(owner, target, "BandageToss_mis.troy", target, 2f, 1f);
                AddBuff("Stun", 1.5f, 1, spell, target, owner);
                ForceMovement(owner, "Spell1", target.Position, 2200, 0, 0, 0);



                missile.SetToRemove();

            }

            public void OnSpellCast(ISpell spell)
            {
            }

            public void OnSpellPostCast(ISpell spell)
            {
            }

            public void OnSpellChannel(ISpell spell)
            {
            }

            public void OnSpellChannelCancel(ISpell spell)
            {
            }

            public void OnSpellPostChannel(ISpell spell)
            {
            }

            public void OnUpdate(float diff)
            {
            }
        }
    }

using System.Numerics;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Spells
{
    public class KogMawQMis : ISpellScript
        {
            ObjAIBase Owner;
            public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
            {
                MissileParameters = new MissileParameters
                {
                    Type = MissileType.Circle
                },
                IsDamagingSpell = true
            };

            public void OnActivate(ObjAIBase owner, Spell spell)


            {
                ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
            }



            public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
            {
                var owner = spell.CastInfo.Owner;
                var APratio = owner.Stats.AbilityPower.Total * 0.5f;
                var damage = 80 * (spell.CastInfo.SpellLevel - 0) + APratio;





                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);




                missile.SetToRemove();

            }


            public void OnUpdate(float diff)
            {
            }
        }
    }

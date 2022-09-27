
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
    public class BrandBlaze : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };


        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;

        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var ownerSkinID = owner.SkinID;
            var targetPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var distance = Vector2.Distance(owner.Position, targetPos);
            FaceDirection(targetPos, owner);

            if (distance >= 1100.0)
            {
                targetPos = GetPointFromUnit(owner, 1100.0f);
            }

            if (ownerSkinID == 5)
            {
                SpellCast(owner, 3, SpellSlotType.ExtraSlots, targetPos, targetPos, false, Vector2.Zero);
            }
            else
            {
                SpellCast(owner, 0, SpellSlotType.ExtraSlots, targetPos, targetPos, false, Vector2.Zero);
            }
        }
    }

    public class BrandBlazeMissile : ISpellScript
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
            var APratio = owner.Stats.AbilityPower.Total * 0.65f;
            var damage = 80 * (spell.CastInfo.SpellLevel - 0) + APratio;


            if (target.HasBuff("BrandPassive"))
            {
                AddBuff("Stun", 2f, 1, spell, target, owner);

            }
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            AddParticleTarget(owner, target, "BrandBlaze_mis.troy", target, 1f, 1f);
            AddBuff("BrandPassive", 4f, 1, spell, target, owner, false);




            missile.SetToRemove();

        }


        public void OnUpdate(float diff)
        {
        }
    }
}

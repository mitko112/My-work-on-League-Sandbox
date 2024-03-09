using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.API;
using GameMaths;

namespace Spells
{
    public class FizzMarinerDoom : ISpellScript
    {
        ObjAIBase Owner;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;

        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var ownerSkinID = owner.SkinID;
            var targetPos = GetPointFromUnit(owner, 1150f);
            var trueCoords = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            SpellCast(owner, 4, SpellSlotType.ExtraSlots, trueCoords, targetPos, true, owner.Position);
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }


    public class FizzMarinerDoomMissile : ISpellScript
    {
        Buff HandlerBuff;
        Minion Fish;
        bool hasHitEnemy = false;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true
            // TODO
        };
        AttackableUnit Target;
        public SpellSector BindSector;

        //Vector2 direction;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });
            
        }


        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            float ap = owner.Stats.AbilityPower.Total;
            float damage = 75 + (spell.CastInfo.SpellLevel - 1) * 40 + ap;
            if (missile is SpellCircleMissile circleMissle && circleMissle.ObjectsHit.Count == 1)
            {
                missile.SetToRemove();

                AddParticleTarget(owner, target, "Fizz_UltimateMissile_Orbit.troy", target, 1.5f);
                AddParticleTarget(owner, target, "Fizz_Ring_Green.troy", target, 1.5f);
                AddBuff("FizzChurnTheWatersCling", 1.5f, 1, spell, target, owner);
                hasHitEnemy = true;
            }


        }
        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        { 
        }

    }

}

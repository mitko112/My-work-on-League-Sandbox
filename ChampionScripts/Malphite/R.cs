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
    public class UFSlash : ISpellScript
    {
        Spell spell;

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnMoveSuccess.AddListener(this, owner, OnMoveEnd, false);
        }

       

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var dist = Vector2.Distance(current, spellPos);

            if (dist > 1200.0f)
            {
                dist = 1200.0f;
            }

            FaceDirection(spellPos, owner, true);
            var trueCoords = GetPointFromUnit(owner, dist);
            var time = dist / 2300f;
            PlayAnimation(owner, "Spell4");
            AddParticleTarget(owner, owner, "Malphite_Base_UnstoppableForce_cas.troy", owner, 0.5f);
            AddParticle(owner, null, ".troy", owner.Position);
            ForceMovement(owner, null, trueCoords, 2300, 0, 0, 0);
        }
        public void OnMoveEnd(AttackableUnit owner)
        {
            if (owner is ObjAIBase c)
            {
                StopAnimation(c, "Spell4");
                AddParticle(c, null, ".troy", c.Position);
                AddParticle(c, null, "Malphite_Base_UnstoppableForce_tar.troy", c.Position);

                var units = GetUnitsInRange(c.Position, 260f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is ObjBuilding || units[i] is BaseTurret))
                    {
                        var RLevel = c.GetSpell("UFSlash").CastInfo.SpellLevel;
                        var damage = 200 + (100 * (RLevel - 1)) + (c.Stats.AbilityPower.Total * 1f);
                        units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                        AddParticleTarget(c, units[i], "Malphite_Base_UnstoppableForce_stun.troy", units[i], 1f);
                        AddParticleTarget(c, units[i], "Malphite_Base_UnstoppableForce_tar.troy", units[i], 1f);
                        ForceMovement(units[i], "RUN", new Vector2(units[i].Position.X + 10f, units[i].Position.Y + 10f), 13f, 0, 16.5f, 0);
                    }
                }
            }
        }
       

    }

}

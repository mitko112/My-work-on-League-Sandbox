using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class GragasR : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            FaceDirection(pos, target);
            var pos2 = GetPointFromUnit(target, -500);
            ForceMovement(target, "run", pos2, 1000, 0, 0, 0);
        var owner = spell.CastInfo.Owner;

            var ap = owner.Stats.AbilityPower.Total * 0.7f;
            var damage = 200 + spell.CastInfo.SpellLevel + ap;

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
        }
        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        Vector2 pos;
        public ISpellSector DamageSector;
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {

            var spellPos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            var diff = new Vector2(owner.Position.X - spellPos.X, owner.Position.Y - spellPos.Y);
            pos = spellPos;

            DamageSector = spell.CreateSpellSector(new SectorParameters
            {
                Tickrate = 1,
                Length = 350f,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
            CreateTimer(1.0f, () => { var pre = AddParticle(owner, null, "Gragas_Base_R_End.troy", spellPos, lifetime: 4.0f); DamageSector.SetToRemove(); });
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


using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using GameServerCore.Enums;

namespace Spells
{
    public class BusterShot : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            // TODO
        MissileParameters = new MissileParameters()
        {
            Type = MissileType.Target
        }
        };
        ObjAIBase Owner;
        Spell Spell;
        public void OnActivate(ObjAIBase owner,Spell spell)
        {
            Owner = owner;
            Spell = spell;
            ApiEventManager.OnSpellHit.AddListener(this, spell, OnSpellHit, false);
        }
        public void OnSpellHit(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            FaceDirection(Owner.Position, target);

            AddParticleTarget(Owner, target, "tristana_bustershot_tar", target, 1f, 1f);
            AddParticleTarget(Owner, Owner, "BusterShot_cas.troy", Owner, 1f, 1f);
            float Damage = 200 + 100 * Spell.CastInfo.SpellLevel + Owner.Stats.AbilityPower.Total * 1.5f;
            target.TakeDamage(Owner, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            ForceMovement(target, "RUN", GetPointFromUnit(target, -(400 + 200 * Spell.CastInfo.SpellLevel)), 2200, 0, 0, 0);
            missile.SetToRemove();
        }
    }
}


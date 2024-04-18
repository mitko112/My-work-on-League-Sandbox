using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;


namespace Spells
{
    public class FioraRiposte : ISpellScript
    {
        ObjAIBase Fiora;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Fiora = owner = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, false);
        }
        public void OnLevelUp(Spell spell)
        {
            AddBuff("FioraRiposteBuff", 25000.0f, 1, spell, Fiora, Fiora, true);
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("FioraRiposte", 1.5f, 1, spell, Fiora, Fiora);
        }
    }
    public class FioraRiposteMissile : ISpellScript
    {
        float Damage;
        ObjAIBase Fiora;
        SpellMissile Missile;
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata() { TriggersSpellCasts = true };
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Fiora = owner = spell.CastInfo.Owner as Champion;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Missile = spell.CreateSpellMissile(new MissileParameters { Type = MissileType.Target, });
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            AddParticleTarget(Fiora, target, "FioraRiposte_tar.troy.troy", Fiora, 1, 1, "Center");
            Damage = 10 + (50 * Fiora.Spells[1].CastInfo.SpellLevel) + Fiora.Stats.AbilityPower.FlatBonus;
            target.TakeDamage(Fiora, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
            missile.SetToRemove();
        }
    }
    public class FioraRiposteMissileTower : FioraRiposteMissile { }
    public class FioraRiposteMissileTowerChs : FioraRiposteMissile { }
    public class FioraRiposteMissileTowerDom : FioraRiposteMissile { }
}
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;

namespace Spells
{
    public class PowerFist : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDeathRecapSource = true,
            NotSingleTargetSpell = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }
        Spell thisSpell;
        public void OnHitUnit(DamageData  damageData)
        {
            LogDebug("yo");
            CreateTimer(0.1f, () => { thisSpell.CastInfo.Owner.RemoveBuffsWithName("PowerFist"); });
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            ApiEventManager.OnHitUnit.AddListener(this, owner, OnHitUnit, true);
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
            thisSpell = spell;
            var owner = spell.CastInfo.Owner;
            AddBuff("PowerFist", 10.0f, 1, spell, owner, owner);
        }

        
        
    }
}

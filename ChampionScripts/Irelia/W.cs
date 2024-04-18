
using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;

namespace Spells
{
    public class IreliaHitenStyle : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            // TODO
        };
        ObjAIBase Owner;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Owner = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, OnHitUnit, false);
        }
        public void OnHitUnit(DamageData data)
        {
            var heal = 5;
            Owner.Stats.CurrentHealth += heal;
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("IreliaHitenStyleCharged", 6.0f, 1, spell, owner, owner);
            AddParticleTarget(owner, owner, "irelia_hitenStlye_active_glow.troy", owner, 6f);
            
        }

    }
}
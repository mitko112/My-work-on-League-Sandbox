
using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;

namespace Spells
{
    public class VayneSilveredBolts : ISpellScript
    {
        Spell Spell;
        ObjAIBase Owner;
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Spell = spell;
            Owner = spell.CastInfo.Owner;
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, true);

            AddBuff("VayneSilveredBolts", 1f, 1, spell, owner, owner, true);
        }
        public void OnLevelUp (Spell spell)
        {
            ApiEventManager.OnHitUnit.AddListener(this, spell.CastInfo.Owner, OnHitUnit, false);
        }
        public void OnHitUnit(DamageData damageData)
        {
            var target = damageData.Target;
            AddBuff("VayneSilveredBolts", 3f, 1, Spell, target, Owner);
        }
      
       
        }
    }

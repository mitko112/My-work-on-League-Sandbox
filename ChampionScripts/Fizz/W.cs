using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerLib.GameObjects.AttackableUnits;

namespace Spells
{
    public class FizzSeastonePassive : ISpellScript
    {
        
        Spell Spell;
        ObjAIBase Owner;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };
        

        public void OnActivate(ObjAIBase owner, Spell spell)
        
            {
                
                Spell = spell;
                Owner = owner;
                ApiEventManager.OnLevelUpSpell.AddListener(this, spell, AddFizzPassive, false);

                AddBuff("FizzMalison", 1f, 1, spell, Owner, Owner, true);
            }


        public void AddFizzPassive(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, OnHitUnit, false);

        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnHitUnit(DamageData damageData)

        {
            var target = damageData.Target;
            AddBuff("FizzMalison", 3f, 1, Spell, target, Owner);
        }
        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("FizzSeastoneActive", 5f, 1, spell, owner, owner);

        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
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
            //ApiEventManager.OnHitUnit.AddListener(this, daowner, TargetTakePoison, false);
        }
    }
}

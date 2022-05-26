using System;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;

namespace Spells
{
    public class FizzSeastonePassive : ISpellScript
    {
        IAttackableUnit Target;
        ISpell Spell;
        IObjAiBase Owner;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            
            Spell = spell;
            Owner = owner;
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnlevelUp, true);
            AddBuff("FizzSeastoneTrident", 1f, 1, Spell, owner, owner, true);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            
            
        }



        public void OnlevelUp(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            ApiEventManager.OnHitUnit.AddListener(this, spell.CastInfo.Owner, OnHitUnit, false);
        }
        public void OnHitUnit(IDamageData damageData)


        {
            var target = damageData.Target;
            AddBuff("FizzSeastoneTrident", 3f, 1, Spell, target, Owner);
        }


        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("FizzSeastoneActive", 5f, 1, spell, owner, owner);

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
            //ApiEventManager.OnHitUnit.AddListener(this, daowner, TargetTakePoison, false);
        }
    }
}
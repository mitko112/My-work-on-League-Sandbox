using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Linq;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Logging;
using LeagueSandbox.GameServer.API;
using System;

namespace Spells
{
    public class GarenW : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        private ObjAIBase owner;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            this.owner = owner;

            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUpSpell);
        }

        private void OnLevelUpSpell(Spell spell)
        {
            //Second slot, W spell
            if (spell.CastInfo.SpellSlot != 1)
            {
                return;
            }

            //When it levels up and becomes level 1 it means that it was just learned
            if(spell.CastInfo.SpellLevel != 1)
            {
                return;
            }

            LoggerProvider.GetLogger().Warn("Garen w passive add");
            AddBuff("GarenWPassive", 1, 1, spell, owner, owner, true);
        }

        public void OnSpellPostCast(Spell spell)
        {
            float duration = 2 + spell.CastInfo.SpellLevel - 1;
            AddBuff("GarenW", duration, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);
        }
    }
}


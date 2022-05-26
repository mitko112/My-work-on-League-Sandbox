using System.Collections.Generic;
using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain;

namespace Spells
{
    public class VayneSilveredBolts : ISpellScript
    {
        ISpell Spell;
        IObjAiBase Owner;
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            Spell = spell;
            Owner = spell.CastInfo.Owner;
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, true);

            AddBuff("VayneSilveredBolts", 1f, 1, spell, owner, owner, true);
        }
        public void OnLevelUp (ISpell spell)
        {
            ApiEventManager.OnHitUnit.AddListener(this, spell.CastInfo.Owner, OnHitUnit, false);
        }
        public void OnHitUnit(IDamageData damageData)
        {
            var target = damageData.Target;
            AddBuff("VayneSilveredBolts", 3f, 1, Spell, target, Owner);
        }
        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
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
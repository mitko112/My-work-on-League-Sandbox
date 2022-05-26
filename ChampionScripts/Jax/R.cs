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
    public class JaxRelentlessAssault : ISpellScript
    {
        IObjAiBase Owner;
        ISpell Spell;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            
            Spell = spell;
            Owner = spell.CastInfo.Owner;
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, true);
             AddBuff("JaxRelentlessAttack", 8f, 1, spell, owner, owner, true);

        }
        public void OnLevelUp(ISpell spell)

        {
            
             ApiEventManager.OnHitUnit.AddListener(this, spell.CastInfo.Owner, OnHitUnit, false);
            }

        public void OnHitUnit(IDamageData damageData)
        {
            var target = damageData.Target;
           AddBuff("JaxRelentlessAttack", 2.5f, 1, Spell, target, Owner, false);
        }
        
    
        

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
            var Owner = spell.CastInfo.Owner;

            AddBuff("JaxRelentlessAssaultAS", 8f, 1, spell, Owner, Owner, false);
            AddParticleTarget(Owner, Owner, "JaxRelentlessAssault_buf.troy", Owner, 8f, 1f);
            
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
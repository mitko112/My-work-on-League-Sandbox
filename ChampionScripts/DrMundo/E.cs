
using System.Numerics;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Spells
{
    public class Masochism : ISpellScript
    {
        ObjAIBase Owner;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Owner = owner;
        }

        

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            float SelfDMG = 15 + 10f * spell.CastInfo.SpellLevel;

            if (owner.Stats.CurrentHealth > SelfDMG)
            {
                owner.Stats.CurrentHealth -= SelfDMG;
            }
            else
            {
                owner.Stats.CurrentHealth = 1f;
            }

            AddBuff("Masochism", 5f, 1, spell, Owner, Owner, false);

        }
    }
}


using System.Numerics;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Spells
{
    public class BurningAgony : ISpellScript
    {
        ObjAIBase Owner;
        float timeSinceLastTick = 1000f;
        Spell Spell;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Owner = owner;
            Spell = spell;
        }

        public void OnSpellPostCast(Spell spell)
        {
            if (!Owner.HasBuff("BurningAgony"))
            {
                AddBuff("BurningAgony", 1, 1, spell, Owner, Owner, true);
                timeSinceLastTick = 1000f;
                spell.SetCooldown(0.5f, true);
            }
            else
            {
                RemoveBuff(Owner, "BurningAgony");
            }
        }

        
        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;
            if (timeSinceLastTick >= 1000.0f && Owner != null && Spell != null && Owner.HasBuff("BurningAgony"))
            {
                float selfDMG = 5f + (5f * Spell.CastInfo.SpellLevel);
                if (Owner.Stats.CurrentHealth > selfDMG)
                {
                    Owner.Stats.CurrentHealth -= selfDMG;
                }
                else
                {
                    Owner.Stats.CurrentHealth = 1f;
                }
                timeSinceLastTick = 0f;
            }
        }
    }
}


using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;


namespace Spells
{
    public class BloodScent : ISpellScript
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
            if (!Owner.HasBuff("BloodScent"))
            {
                AddBuff("BloodScent", 1, 1, spell, Owner, Owner, true);
                timeSinceLastTick = 1000f;
                spell.SetCooldown(4f, true);
            }
            else
            {
                RemoveBuff(Owner, "BloodScent");
            }
        }

    }
}


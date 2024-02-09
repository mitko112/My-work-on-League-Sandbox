
using System.Numerics;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;

namespace Spells
{
    public class Sadism : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };


        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            AddParticleTarget(owner, owner, "dr_mundo_sadism_cas.troy", owner, 1);
            AddParticleTarget(owner, owner, "dr_mundo_sadism_cas_02.troy", owner, 1);
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;


            AddBuff("Sadism", 12f, 1, spell, owner, owner, false);
            owner.Stats.CurrentHealth *= 0.8f; //20% Current health
        }

       
    }
}

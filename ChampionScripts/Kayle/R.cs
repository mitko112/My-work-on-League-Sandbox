using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class JudicatorIntervention  : ISpellScript
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




            var target = spell.CastInfo.Targets[0].Unit;
            AddBuff("JudicatorIntervention", 2f, 1, spell, target, Owner, false);
            
        }

        
    }
}

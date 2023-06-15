using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class JudicatorRighteousFury : ISpellScript
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




            
            AddBuff("JudicatorRighteousFury", 10f, 1, spell, Owner, Owner, false);
            
        }

        
    }
}

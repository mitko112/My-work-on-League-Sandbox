using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;

namespace Spells
{
    public class CharScriptCassiopeia : ISpellScript
    {
        private ObjAIBase _owner;

        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata();
        public void OnActivate(Spell spell)
        {
            _owner = spell.CastInfo.Owner as ObjAIBase;

            // Listen for spell casts by this champion
            ApiEventManager.OnSpellCast.AddListener(this, spell, OnSpellCast, false);
        }

        private void OnSpellCast(Spell spell)
        {
            if (spell.CastInfo.Owner != _owner)
                return;

            // Add / refresh the passive buff
            AddBuff(
                "CassiopeiaPassiveMana",
                5.0f,     // duration
                1,        // stacks added
                spell,
                _owner,
                _owner
            );
        }

        public void OnDeactivate(Spell spell)
        {
            ApiEventManager.OnSpellCast.RemoveListener(this);
        }
    }
}
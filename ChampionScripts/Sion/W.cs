
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class SionW: ISpellScript
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

            AddBuff("SionWShield", 10f, 1, spell, Owner, Owner, false);
            AddBuff("SionWSwitch", 10f, 1, spell, Owner, Owner, false);

            
        }


       
    }
}
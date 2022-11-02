
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Passives
{
    public class Focus : ICharScript
    {

        Spell originspell;
        ObjAIBase ownermain;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            originspell = spell;
            ownermain = owner;
            AddBuff("PassiveMSVayne", float.MaxValue, 1, spell, owner, spell.CastInfo.Owner);
        }

  
    }
}


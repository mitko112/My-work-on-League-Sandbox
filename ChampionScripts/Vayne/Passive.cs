using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Scripting.CSharp;

namespace Passives
{
    public class Focus : ICharScript
    {

        ISpell originspell;
        IObjAiBase ownermain;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            originspell = spell;
            ownermain = owner;
            AddBuff("PassiveMSVayne", float.MaxValue, 1, spell, owner, spell.CastInfo.Owner);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}


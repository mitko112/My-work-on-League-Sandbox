using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static LeaguePackets.Game.Common.CastInfo;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class GarenQHaste : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private Particle hasteParticle1;
        private Particle hasteParticle2;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;

            StatsModifier.MoveSpeed.PercentBonus += 0.35f;
            unit.AddStatModifier(StatsModifier);

            if(!HasBuff(unit, "GarenE"))
            {
                OverrideAnimation(unit, "Run_Spell1", "RUN");
            }

            hasteParticle1 = AddParticleTarget(owner, owner, "Garen_Base_Q_Buf.troy", unit, bone: "chest");
            hasteParticle2 = AddParticleTarget(owner, owner, "Garen_Base_Q_Cas.troy", unit, bone: "chest");
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(hasteParticle1);
            RemoveParticle(hasteParticle2);

            if(!HasBuff(unit, "GarenE"))
            {
                ClearOverrideAnimation(unit, "RUN");
            }
        }
    }
}

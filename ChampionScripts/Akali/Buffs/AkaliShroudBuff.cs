using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;
using static LeaguePackets.Game.Common.CastInfo;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;

namespace Buffs
{
    internal class AkaliWDebuff : IBuffGameScript
    {
        Particle Slow;
        public  BuffScriptMetaData BuffMetaData { get; } = new()
        {
            BuffType = BuffType.AURA,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public  StatsModifier StatsModifier { get; protected set; } = new();
        Spell Spell;
        public  void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell )
        {
            Spell = ownerSpell;
            var owner = Spell.CastInfo.Owner;
            Slow = AddParticleTarget(unit,  unit,"Global_Slow", unit, buff.Duration);
            StatsModifier.MoveSpeed.PercentBonus -= 0.14f + 0.04f * (owner.GetSpell("AkaliSmokeBomb").CastInfo.SpellLevel);
            unit.AddStatModifier(StatsModifier);
        }
        public  void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(Slow);
        }
    }
}


using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class CassiopeiaPassiveStacks : IBuffGameScript
    {
        public StatsModifier StatsModifier { get; } = new();

        public BuffScriptMetaData BuffMetaData { get; } = new()
        {
            BuffType = BuffType.COUNTER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 5,
            IsHidden = false
        };
    }
}
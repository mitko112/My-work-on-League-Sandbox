using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class HecarimQCooldown : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } =
            new BuffScriptMetaData
            {
                BuffType = BuffType.COMBAT_ENCHANCER,
                BuffAddType = BuffAddType.STACKS_AND_RENEWS,
                MaxStacks = 2
            };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            owner.GetSpell("HecarimRapidSlash").LowerCooldown(1f);
        }

        // ❌ DO NOT RESTORE COOLDOWN HERE
        public void OnDeactivate(AttackableUnit unit, Buff buff) { }

        }
    }

using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using            GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using System.Linq;
using LeagueSandbox.GameServer.Logging;
using System.Threading.Tasks;
using System;
using System.Threading;
using CharScripts;

namespace Buffs
{
    internal class GarenPassive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; }

        private Buff thisBuff;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;

            AddBuff("GarenPassiveHeal", 99999f, 1, ownerSpell, unit, ownerSpell.CastInfo.Owner);

            OnUpdateStats(unit);
            ApiEventManager.OnUpdateStats.AddListener(this, unit, OnUpdateStats);
        }

        public void OnUpdateStats(AttackableUnit unit, float diff = 0)
        {
            float healthPercentage = GarenPassiveHeal.HEALTH_PERCENTAGES[CharScriptGaren.GetCorrectLevelIndex(unit.Stats.Level)];
            float healthRegenPerSecond = healthPercentage * unit.Stats.HealthPoints.Total;
            float healthRegenPerFiveSeconcs = healthRegenPerSecond * 5f;
            thisBuff.SetToolTipVar(0, healthRegenPerFiveSeconcs);
            thisBuff.SetToolTipVar(1, CharScriptGaren.GetCooldownForLevel(unit.Stats.Level));
            thisBuff.SetToolTipVar(2, healthPercentage * 100f);
        }
    }
}

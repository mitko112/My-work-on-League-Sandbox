using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Logging;
using LeagueSandbox.GameServer.GameObjects;

namespace Buffs
{
    internal class HecarimPassive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private Buff thisBuff;

        private float currentBonusMoveSpeed = -1f;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;

            OnUpdateStats(unit, 0);
            ApiEventManager.OnUpdateStats.AddListener(this, unit, OnUpdateStats);
        }

        public void OnUpdateStats(AttackableUnit unit, float diff)
        {
            float bonusMoveSpeed = unit.Stats.MoveSpeed.Total - unit.Stats.MoveSpeed.BaseValue;
            if (currentBonusMoveSpeed != bonusMoveSpeed)
            {
                unit.RemoveStatModifier(StatsModifier);

                currentBonusMoveSpeed = bonusMoveSpeed;

                StatsModifier.AttackDamage.FlatBonus = bonusMoveSpeed * 0.2f;

                unit.AddStatModifier(StatsModifier);

                thisBuff.SetToolTipVar(0, StatsModifier.AttackDamage.FlatBonus);
                thisBuff.SetToolTipVar(1, 20);
            }

        }
    }
}
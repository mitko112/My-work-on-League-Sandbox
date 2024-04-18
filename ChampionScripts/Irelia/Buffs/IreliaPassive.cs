using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using System.Linq;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.Logging;


namespace Buffs
{
    internal class IreliaPassive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        private readonly int[] TENACITIES = new int[] { 10, 25, 40};

        private readonly float CHECK_UNITS_NEARBY_COOLDOWN = 500f;
        private float checkUnitsNearbyCurrentTimer = 0;

        private int enemyChampionsNearby = -1;

        private AttackableUnit thisUnit;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisUnit = unit;
        }

        public void OnUpdate(float diff)
        {
            checkUnitsNearbyCurrentTimer += diff;
            if (checkUnitsNearbyCurrentTimer >= CHECK_UNITS_NEARBY_COOLDOWN)
            {
                checkUnitsNearbyCurrentTimer = 0;

                PassiveCheck();
            }
        }

        private void PassiveCheck()
        {
            List<Champion> championsNearby = ApiFunctionManager.GetChampionsInRange(thisUnit.Position, 1000f, true);

            int newEnemyChampionsNearby = championsNearby.Where(u => u.Team != thisUnit.Team).Count();

            if (enemyChampionsNearby != newEnemyChampionsNearby)
            {
                enemyChampionsNearby = newEnemyChampionsNearby;

                PassiveUpdate();
            }
        }

        private void PassiveUpdate()
        {
            thisUnit.RemoveStatModifier(StatsModifier);
            StatsModifier = new StatsModifier();
            if (enemyChampionsNearby >= 3)
            {
                StatsModifier.Tenacity.FlatBonus += (TENACITIES[2] / 100f);
            }
            else if (enemyChampionsNearby == 2)
            {
                StatsModifier.Tenacity.FlatBonus += (TENACITIES[1] / 100f);
            }
            else if (enemyChampionsNearby == 1)
            {
                StatsModifier.Tenacity.FlatBonus += (TENACITIES[0] / 100f);
            }
            else
            {
                StatsModifier.Tenacity.FlatBonus = 0;
            }
            thisUnit.AddStatModifier(StatsModifier);
        }
    }
}

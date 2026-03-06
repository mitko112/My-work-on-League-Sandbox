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
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.Logging;
using System;

namespace Buffs
{
    class PassiveFury : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private readonly float OUT_OF_COMBAT_TIME = 8000f;

        private ObjAIBase owner;

        private float timeSinceLastCombat = 0f;
        private float drainTimer = 0f;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            owner = ownerSpell.CastInfo.Owner;

            ApiEventManager.OnTakeDamage.AddListener(this, owner, RefreshCombat);
        }

        private void RefreshCombat(DamageData data)
        {
            LoggerProvider.GetLogger().Info("Combat detected");
            timeSinceLastCombat = 0f;
            drainTimer = 0f;
        }


        public void OnUpdate(float diff)
        {
            timeSinceLastCombat += diff;
            LoggerProvider.GetLogger().Info("Time since last combat: " + timeSinceLastCombat);

            if (timeSinceLastCombat >= OUT_OF_COMBAT_TIME)
            {
                DrainBlood(diff);
            }
        }

        private void DrainBlood(float diff)
        {
            drainTimer += diff;
            if (drainTimer >= 1000f)
            {
                LoggerProvider.GetLogger().Info("Draining mana");
                float drainAmount = 5f;
                owner.Stats.CurrentMana = Math.Max(owner.Stats.CurrentMana - drainAmount, 0f);

                drainTimer = 0f;
            }
        }
    }
}
        

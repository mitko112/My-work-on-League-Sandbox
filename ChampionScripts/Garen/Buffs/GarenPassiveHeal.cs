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
using CharScripts;

namespace Buffs
{
    internal class GarenPassiveHeal : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; }

        float outOfCombatTimer = 9000f;
        float healingTimer = 0f;

        private Particle healParticle = null;

        public static readonly float[] HEALTH_PERCENTAGES = { 0.004f, 0.008f, 0.02f };

        ObjAIBase unit;
        Spell spell;
        Buff thisBuff;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            this.spell = ownerSpell;
            this.thisBuff = buff;
            this.unit = ownerSpell.CastInfo.Owner;

            healParticle = AddParticleTarget(this.unit, this.unit, "Garen_Base_P_Heal.troy", this.unit, 99999f, bone: "chest");

            ApiEventManager.OnTakeDamage.AddListener(this, this.unit, OnTakeDamage);
        }

        public void OnTakeDamage(DamageData data)
        {
            if (!CharScriptGaren.ShouldPassiveTurnOff(unit, data))
            {
                return;
            }

            RemoveParticle(healParticle);
            AddBuff("GarenPassiveCooldown", CharScriptGaren.GetCooldownForLevel(unit.Stats.Level), 1, spell, unit, unit);
            RemoveBuff(thisBuff);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnTakeDamage.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
            healingTimer += diff;
            if (healingTimer > 1000f)
            {
                float healAmount = HEALTH_PERCENTAGES[CharScriptGaren.GetCorrectLevelIndex(unit.Stats.Level)] * unit.Stats.HealthPoints.Total;
                unit.TakeHeal(unit, healAmount);
                healingTimer = 0;
            }
        }
    }
}

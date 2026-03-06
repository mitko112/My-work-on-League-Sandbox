using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace CharScripts
{
    public class CharScriptTryndamere: ICharScript
    {
        Spell Spell;
        ObjAIBase Owner;

        float outOfCombatTimer = 0f;
        bool hasEverBeenInCombat = false;

        const float OUT_OF_COMBAT_DELAY = 12f;      // seconds
        const float FURY_DECAY_PER_SECOND = 2.5f;   // 5 per 2 seconds

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Spell = spell;
            Owner = owner;

            // Renekton starts with 0 fury
            Owner.Stats.CurrentMana = 0;

            ApiEventManager.OnHitUnit.AddListener(this, owner, OnHitUnit, false);
            ApiEventManager.OnTakeDamage.AddListener(this, owner, OnCombatEvent, false);
            ApiEventManager.OnDealDamage.AddListener(this, owner, OnCombatEvent, false);
        }

        /* =======================
         * AUTO ATTACK FURY GAIN
         * ======================= */
        public void OnHitUnit(DamageData damageData)
        {
            var owner = Spell.CastInfo.Owner;

            owner.Stats.CurrentMana += 5;

            if (owner.Stats.CurrentHealth < owner.Stats.HealthPoints.Total * 0.5f)
                owner.Stats.CurrentMana += 7.5f;

            AddBuff("BattleFury", 99999f, 1, Spell, Owner, Owner);

            // Always reset combat timer when hitting
            EnterCombat();
        }

        /* =======================
         * COMBAT FLAG
         * ======================= */
        public void OnCombatEvent(DamageData damageData)
        {
            EnterCombat();
        }

        // 🔴 THIS IS WHAT Q / W / E BUFFS MUST CALL
        public void OnFuryGainedFromSpell()
        {
            EnterCombat();
        }

        void EnterCombat()
        {
            hasEverBeenInCombat = true;
            outOfCombatTimer = 0f;
        }

        /* =======================
         * UPDATE LOOP
         * ======================= */
        public void OnUpdate(float diff)
        {
            if (Owner == null || Owner.IsDead)
                return;

            // 🔒 Never decay if Renekton was never active
            if (!hasEverBeenInCombat)
                return;

            float deltaSeconds = diff / 1000f;
            outOfCombatTimer += deltaSeconds;

            if (outOfCombatTimer >= OUT_OF_COMBAT_DELAY &&
                Owner.Stats.CurrentMana > 0)
            {
                Owner.Stats.CurrentMana -= FURY_DECAY_PER_SECOND * deltaSeconds;

                if (Owner.Stats.CurrentMana < 0)
                    Owner.Stats.CurrentMana = 0;
            }
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
            ApiEventManager.OnTakeDamage.RemoveListener(this);
            ApiEventManager.OnDealDamage.RemoveListener(this);
        }
    }
}

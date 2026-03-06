using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace CharScripts
{
    public class CharScriptMalphite : ICharScript
    {
        ObjAIBase Owner;
        Spell PassiveSpell;

        float outOfCombatTimer = 0f;
        bool shieldActive = false;
        Particle brokenParticle;
        float[] REFRESH_TIMES = { 10f, 8f, 6f }; // spell levels 1/2/3

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Owner = owner;
            PassiveSpell = spell;

            ApiEventManager.OnTakeDamage.AddListener(this, owner, OnTakeDamage);

            // 🪨 START WITH SHIELD READY
            AddBuff("MalphiteGraniteShield", 25000f, 1, PassiveSpell, Owner, Owner);
            shieldActive = true;
            outOfCombatTimer = 0f;
        }

        void OnTakeDamage(DamageData damageData)
        {
            if (damageData.Target != Owner)
                return;

            outOfCombatTimer = 0f;
            shieldActive = false;

            // 🔥 show broken-shield particle (START cooldown visual)
            if (brokenParticle == null)
            {
                brokenParticle = AddParticleTarget(
                    Owner,
                    Owner,
                    "Malphite_Base_Enrage_glow.troy",
                    Owner,10f
                );
            }
        }
        public void OnUpdate(float diff)
        {
            if (Owner == null || Owner.IsDead)
                return;

            float deltaSeconds = diff / 1000f;
            outOfCombatTimer += deltaSeconds;

            int level = Owner.Stats.Level;

            float refreshTime;
            if (level < 6)
                refreshTime = 10f;
            else if (level < 11)
                refreshTime = 8f;
            else
                refreshTime = 6f;

            if (!shieldActive && outOfCombatTimer >= refreshTime)
            {
                AddBuff("MalphiteGraniteShield", 25000f, 1, PassiveSpell, Owner, Owner);
                shieldActive = true;

                // 🪨 shield back up → remove broken visual
                if (brokenParticle != null)
                {
                    RemoveParticle(brokenParticle);
                    brokenParticle = null;
                }
            }
        }





        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnTakeDamage.RemoveListener(this);
        }
    }
}
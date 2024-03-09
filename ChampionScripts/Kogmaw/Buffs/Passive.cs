
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class KogmawPassive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };
       
        
        public StatsModifier StatsModifier { get; private set; }

        private float timer = 0f;
        private float tickCount = 0f;
        private Champion champion;
        private Buff thisBuff;
        float atk;
        AttackableUnit Unit;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnDeath.AddListener(this, unit, OnDeath, true);
            champion = unit as Champion;
            thisBuff = buff;
            Unit = unit;

            SetStatus(unit, StatusFlags.CanAttack, false);
            //These Particles aren't working
            //p = AddParticleTarget(unit, null, "Sion_Skin01_Passive_Skin.troy", unit, buff.Duration);
            //p2 = AddParticleTarget(unit, null, "Sion_Skin01_Passive_Ax.troy", unit, buff.Duration);

            for (byte i = 0; i < 4; i++)
            {
                if (champion != null)
                {
                    SealSpellSlot(champion, SpellSlotType.SpellSlots, i, SpellbookType.SPELLBOOK_CHAMPION, true);
                }
            };
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            string[] originalAbilities = new string[] { "KogMawQ", "KogMawBioArcaneBarrage", "KogMawVoidOoze", "KogMawLivingArtillery" };

            for (byte i = 0; i < 4; i++)
            {
                if (champion != null)
                {
                    SealSpellSlot(champion, SpellSlotType.SpellSlots, i, SpellbookType.SPELLBOOK_CHAMPION, false);
                }
            }

            unit.TakeDamage(unit, 100000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);

            RemoveBuff(unit, "KogmawPassiveDelay");
            RemoveBuff(unit, "KogmawPassive");
            //RemoveParticle(p);
            //RemoveParticle(p2);
        }

        public void OnDeath(DeathData deathData)
        {
            if (thisBuff != null && !thisBuff.Elapsed())
            {
                thisBuff.DeactivateBuff();
            }
        }

        public void OnUpdate(float diff)
        {
            timer += diff;
            if (timer > 250f && champion != null)
            {
                champion.TakeDamage(champion, 1 + champion.Stats.Level + tickCount * (0.7f * (champion.Stats.Level * 0.7f)), DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, false);
                timer = 0;
                tickCount++;
                SetStatus(Unit, StatusFlags.CanAttack, true);
            }
        }
    }
}
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class ObduracyBuff : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        private IObjAiBase Owner;
        private ISpell daspell;
        private IObjAiBase daowner;
		IAttackableUnit Unit;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IBuff thisBuff;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Unit = unit;
			thisBuff = buff;
            Owner = ownerSpell.CastInfo.Owner;
            daowner = Owner;
            daspell = ownerSpell;
			AddParticleTarget(daowner, unit, "Malphite_Enrage_buf", unit, buff.Duration, 1,"L_HAND");
			AddParticleTarget(daowner, unit, "Malphite_Enrage_buf", unit, buff.Duration, 1,"R_HAND");
			AddParticleTarget(daowner, unit, "Malphite_Enrage_glow", unit, buff.Duration, 1,"L_HAND");
			AddParticleTarget(daowner, unit, "Malphite_Enrage_glow", unit, buff.Duration, 1,"R_HAND");
            SealSpellSlot(daowner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true);
            ApiEventManager.OnLaunchAttack.AddListener(this, ownerSpell.CastInfo.Owner, OnLaunchAttack, false);
        }

        public void OnLaunchAttack(ISpell spell)
        {
			if (!thisBuff.Elapsed() && thisBuff != null && Unit != null)
            {
			var Owner = daspell.CastInfo.Owner as IChampion;
			var Elevel = Owner.GetSpell("Obduracy").CastInfo.SpellLevel;
			var AD = Owner.Stats.AbilityPower.Total * 0.1f;
            var Damage = 30 + (15*(Elevel -1)) + AD;
			//target.TakeDamage(owner, Damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
			AddParticleTarget(Owner, spell.CastInfo.Owner.TargetUnit, "TiamatMelee_itm_hydra.troy", Owner,1f,0.7f);               	
			var units = GetUnitsInRange(spell.CastInfo.Owner.TargetUnit.Position, 250f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == Owner.Team || units[i] is IBaseTurret || units[i] is IObjBuilding || units[i] is IInhibitor))
                {
                    units[i].TakeDamage(Owner, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);                          	
                    AddParticleTarget(Owner, units[i], ".troy", Owner, 10f);					
                }
            }
			}
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			ApiEventManager.OnHitUnit.RemoveListener(this, unit as IObjAiBase);
			//beidongdonghua
			//AddParticleTarget(unit, unit, "Malphite_Base_Obduracy_off", unit, buff.Duration, 1);
			SealSpellSlot(ownerSpell.CastInfo.Owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, false);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
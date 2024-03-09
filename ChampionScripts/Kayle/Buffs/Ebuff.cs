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
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Buffs
{
    class JudicatorRighteousFury : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1,
            IsHidden = false
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Spell Spell;
        public SpellSector DamageSector;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Spell = ownerSpell;


            if (unit is ObjAIBase obj)
            {
                ApiEventManager.OnHitUnit.AddListener(this, obj, TargetExecute, false);



                StatsModifier.Range.FlatBonus = StatsModifier.Size.FlatBonus + 400f;
                unit.AddStatModifier(StatsModifier);
            }
        }
        public void TargetExecute(DamageData data)

        {
            var owner = Spell.CastInfo.Owner;
            var target = data.Target; 
            var AP = owner.Stats.AbilityPower.Total * 0.4f;
            var AD = owner.Stats.AttackDamage.Total * 0.2f;
            float damage = 20 * owner.GetSpell("JudicatorRighteousFury").CastInfo.SpellLevel + AP;
            float damagesplash = 20 * owner.GetSpell("JudicatorRighteousFury").CastInfo.SpellLevel + AP + AD;
      
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);

            foreach (var enemy in GetUnitsInRange(target.Position, 400, true))
            {
                if (enemy is ObjAIBase
                    && enemy != target
                    && data.DamageResultType is DamageResultType.RESULT_NORMAL or DamageResultType.RESULT_CRITICAL)

                    
                    enemy.TakeDamage(owner, damagesplash, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);



            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);

        }

    }
}


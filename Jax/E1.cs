using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class JaxCounterStrikeAttack : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } =
            new SpellScriptMetadata
            {
                TriggersSpellCasts = true
            };

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            if (owner == null)
                return;

            var buff = owner.GetBuffWithName("JaxEDodge");

            int dodges = 0;
            if (buff?.BuffScript is Buffs.JaxEDodge dodgeScript)
                dodges = dodgeScript.DodgedAttacks;


           
            float baseDamage = spell.CastInfo.SpellLevel switch
            {
                1 => 50f,
                2 => 75f,
                3 => 100f,
                4 => 125f,
                5 => 150f,
                _ => 50f
            };

            float bonusAD = owner.Stats.AttackDamage.FlatBonus * 0.5f;
            float damage = (baseDamage + bonusAD) * (1f + dodges * 0.2f);

            var units = GetUnitsInRange(owner.Position, 400f, true);
            foreach (var unit in units)
            {
                if (unit.Team == owner.Team ||
                    unit is BaseTurret ||
                    unit is ObjBuilding ||
                    unit is Inhibitor)
                    continue;

                unit.TakeDamage(
                    owner,
                    damage,
                    DamageType.DAMAGE_TYPE_PHYSICAL,
                    DamageSource.DAMAGE_SOURCE_SPELLAOE,
                    false
                );

                AddBuff("Stun", 1.5f, 1, spell, unit, owner);
            }

            RemoveBuff(owner, "JaxEDodge");

            if (owner.HasBuff("JaxESelfcast"))
                RemoveBuff(owner, "JaxESelfcast");

            spell.SetCooldown(14f, false);
        }
    }
}
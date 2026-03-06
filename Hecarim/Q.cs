using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class HecarimRapidSlash : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            bool hitEnemy = false;

            var units = GetUnitsInRange(owner.Position, 425f, true);
            foreach (var unit in units)
            {
                if (unit.Team == owner.Team ||
                    unit is BaseTurret ||
                    unit is ObjBuilding ||
                    unit is Inhibitor)
                    continue;

                hitEnemy = true;

                float ad = owner.Stats.AttackDamage.FlatBonus;
                float damage = 60 * spell.CastInfo.SpellLevel + ad * 0.6f;
                float damageMM = 40 * spell.CastInfo.SpellLevel + ad * 0.4f;

                unit.TakeDamage(
                    owner,
                    unit is Minion || unit is Monster ? damageMM : damage,
                    DamageType.DAMAGE_TYPE_PHYSICAL,
                    DamageSource.DAMAGE_SOURCE_SPELLAOE,
                    false);
                AddParticle(owner, null, "Hecarim_Q_tar.troy", owner.Position, direction: owner.Direction);
                AddParticle(owner, null, "Hecarim_Q.troy", owner.Position, direction: owner.Direction);
                AddParticle(owner, null, "Hecarim_Q_weapon.troy", owner.Position, direction: owner.Direction);
                PlayAnimation(owner, "Spell1_Upperbody", 0.7f);
            }
            if (!hitEnemy)
                return;

            // Just add / refresh the buff
            AddBuff(
                "HecarimQCooldown",
                8f,
                1,
                spell,
                owner,
                owner,
                false
            );
        }
    }
}
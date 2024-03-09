using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;
using GameServerLib.GameObjects.AttackableUnits;
using System.Collections.Generic;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;

namespace Buffs
{
    public class DetonatingShotPassive : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.AURA,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public StatsModifier StatsModifier { get; protected set; } = new();

        ObjAIBase Owner;
        AttackableUnit Unit;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            var owner = ownerSpell.CastInfo.Owner;
            Owner = owner;
            
            ApiEventManager.OnKill.AddListener(this, Owner, Boom, false);
            ApiEventManager.OnKillUnit.AddListener(this, Owner, Boom, false);
        }
        public void Boom(DeathData Data)
        {
            
            AddParticleTarget(Owner, Owner, "DetonatingShot_buf", Owner);
            float Damage = 25 + 25 * Owner.GetSpell("DetonatingShot").CastInfo.SpellLevel + Owner.Stats.AbilityPower.Total * 0.25f;
            
            var units = GetUnitsInRange(Owner.Position, 250f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == Owner.Team || units[i] is BaseTurret || units[i] is ObjBuilding || units[i] is Inhibitor))
                {
                    units[i].TakeDamage(Owner, Damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                }
            }
        }
    }
}

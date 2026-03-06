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
namespace Buffs
{
    internal class PoppyDiplomaticImmunityDmg : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        private ObjAIBase Owner;
        private float damage2;
        Spell Spell;
        Particle p;
        AttackableUnit target;
        ObjAIBase globowner;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var Owner = ownerSpell.CastInfo.Owner;
            globowner = Owner;
            Spell = ownerSpell;
            AddParticleTarget(Owner, Owner, "DiplomaticImmunity_buf.troy", Owner, buff.Duration);
            ApiEventManager.OnPreTakeDamage.AddListener(Owner, unit, VerifyDamage, false);
        }
        public void VerifyDamage(DamageData damage)
        {
            var attacker = damage.Attacker;
            var owner = damage.Target;

            if (!attacker.HasBuff("PoppyDITarget"))
            {
                owner.SetStatus(StatusFlags.Invulnerable, true);
            }
            else
            {
                owner.SetStatus(StatusFlags.Invulnerable, false);
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            ApiEventManager.OnPreTakeDamage.RemoveListener(this, owner as AttackableUnit);
            ApiEventManager.RemoveAllListenersForOwner(owner);
            globowner.SetStatus(StatusFlags.Invulnerable, false);
        }
    }
}
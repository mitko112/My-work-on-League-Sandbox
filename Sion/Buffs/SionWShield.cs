using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class SionWShield : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private ObjAIBase _owner;
        private Buff _buff;
        private Particle _shieldParticle;
        private float _maxAbsorb;

        Spell Spell;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            _buff = buff;
            Spell = ownerSpell;
            _owner = ownerSpell.CastInfo.Owner as ObjAIBase;

            int level = ownerSpell.CastInfo.SpellLevel;
            float ap = _owner.Stats.AbilityPower.Total * 0.9f;

            // Shield values: 100 / 150 / 200 / 250 / 300 (+90% AP)
            float baseShield = 50 + (50 * level); // 100,150,200,250,300
            _maxAbsorb = baseShield + ap;

            _shieldParticle = AddParticleTarget(
                _owner,
                _owner,
                "deathscaress_buf.troy",
                _owner,
                buff.Duration
            );

            ApiEventManager.OnTakeDamage.AddListener(this, _owner, OnTakeDamage);
        }

        private void OnTakeDamage(DamageData damageData)
        {
            if (damageData.Target != _owner)
                return;

            if (_maxAbsorb <= 0)
                return;

            float damage = damageData.PostMitigationDamage;

            if (damage > _maxAbsorb)
            {
                damageData.PostMitigationDamage -= _maxAbsorb;
                _maxAbsorb = 0;
                RemoveBuff(_buff);
            }
            else
            {
                damageData.PostMitigationDamage = 0;
                _maxAbsorb -= damage;
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnTakeDamage.RemoveListener(this);

            if (_shieldParticle != null)
                RemoveParticle(_shieldParticle);

            var owner = ownerSpell.CastInfo.Owner as Champion;

            float ap = owner.Stats.AbilityPower.Total * 0.9f;
            float damage = 50 + (50 * ownerSpell.CastInfo.SpellLevel) + ap;

            AddParticleTarget(owner, owner, "DeathsCaress_nova.troy", owner, 1f);

            var units = GetUnitsInRange(owner.Position, 550f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == owner.Team || units[i] is BaseTurret || units[i] is ObjBuilding || units[i] is Inhibitor))
                {
                    units[i].TakeDamage(owner, damage,
                        DamageType.DAMAGE_TYPE_MAGICAL,
                        DamageSource.DAMAGE_SOURCE_SPELLAOE,
                        false);
                }
            }

            RemoveBuff(owner, "SionWSwitch");
        }

        public void OnUpdate(float diff) { }
    }
}

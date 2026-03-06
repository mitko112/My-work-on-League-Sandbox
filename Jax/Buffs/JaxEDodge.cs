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
using System;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class JaxEDodge : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } =
            new BuffScriptMetaData
            {
                BuffType = BuffType.INTERNAL,
                BuffAddType = BuffAddType.RENEW_EXISTING
            };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public int DodgedAttacks;

        private AttackableUnit _owner;
        private bool _listenerAdded;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            _owner = unit;

            // Only reset on true first application
            if (buff.StackCount == 1)
                DodgedAttacks = 0;

            // 🔒 ADD LISTENER ONCE
            if (!_listenerAdded)
            {
                ApiEventManager.OnTakeDamage.AddListener(
                    this,
                    unit,
                    OnTakeDamage,
                    false
                );
                _listenerAdded = true;
            }
        }

        private void OnTakeDamage(DamageData data)
        {
            if (_owner == null || !_owner.HasBuff("JaxEDodge"))
                return;

            if (data.Target != _owner)
                return;

            // ================================
            // 1️⃣ DODGE BASIC ATTACKS (UNCHANGED)
            // ================================
            if (data.DamageSource == DamageSource.DAMAGE_SOURCE_ATTACK)
            {
                data.DamageResultType = DamageResultType.RESULT_DODGE;
                data.Damage = 0;

                if (DodgedAttacks < 5)
                    DodgedAttacks++;

                return;
            }

            // ================================
            // 2️⃣ REDUCE AOE DAMAGE BY 25%
            // ================================
            if (data.DamageSource == DamageSource.DAMAGE_SOURCE_SPELLAOE)
            {
                data.Damage *= 0.75f;
            }
        }
    }
}
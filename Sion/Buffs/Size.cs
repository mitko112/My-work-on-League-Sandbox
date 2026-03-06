using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class Size : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private ObjAIBase _owner;
        private Random _rng = new Random();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            _owner = unit as ObjAIBase;

            // Size bonus
            StatsModifier.Size.PercentBonus += 0.75f;
            unit.AddStatModifier(StatsModifier);

            ApiEventManager.OnTakeDamage.AddListener(this, _owner, OnTakeDamage);
        }

        private void OnTakeDamage(DamageData damageData)
        {
            if (damageData.Target != _owner)
                return;

            // Only block basic attacks
            if (damageData.DamageSource != DamageSource.DAMAGE_SOURCE_ATTACK)
                return;

            // 40% proc chance
            if (_rng.NextDouble() > 0.4)
                return;

            int level = _owner.Stats.Level;

            float reduction;
            if (level < 7)
                reduction = 30f;
            else if (level < 13)
                reduction = 40f;
            else
                reduction = 50f;

            // Apply flat reduction
            if (damageData.PostMitigationDamage > reduction)
                damageData.PostMitigationDamage -= reduction;
            else
                damageData.PostMitigationDamage = 0f;

            // Optional: feedback particle
            AddParticleTarget(_owner, _owner, "Sion_Base_Passive.troy", _owner, 0.5f);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnTakeDamage.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class GragasQ : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        private Spell spell;
        private ObjAIBase owner;
        private Vector2 position;
        private float elapsed;
        private bool exploded;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            spell = ownerSpell;
            owner = ownerSpell.CastInfo.Owner;
            position = new Vector2(
                ownerSpell.CastInfo.TargetPositionEnd.X,
                ownerSpell.CastInfo.TargetPositionEnd.Z
            );

            elapsed = 0f;
            exploded = false;

            ApiEventManager.OnSpellCast.AddListener(
                this,
                owner.GetSpell("GragasQ"),
                OnRecast
            );
        }

        private void OnRecast(Spell s)
        {
            exploded = true;
        }

        public void OnUpdate(float diff)
        {
            if (exploded)
            {
                Explode();
                return;
            }

            elapsed += diff / 1000f;

            if (elapsed >= 4f)
            {
                exploded = true;
            }
        }

        private void Explode()
        {
            ApiEventManager.OnSpellCast.RemoveListener(this);

            int level = spell.CastInfo.SpellLevel;
            float ap = owner.Stats.AbilityPower.Total;

            // Fermentation scaling (0–2s → 100%–150%)
            float scale = MathF.Min(elapsed / 2f, 1f);
            float multiplier = 1f + 0.5f * scale;

            float[] baseDamage = { 80, 120, 160, 200, 240 };
            float[] baseSlow = { 30, 35, 40, 45, 50 };

            float damage = (baseDamage[level - 1] + 0.6f * ap) * multiplier;
            float slow = baseSlow[level - 1] * multiplier;

            var enemies = GetUnitsInRange(position, 250f, true);

            foreach (var enemy in enemies)
            {
                enemy.TakeDamage(
                    owner,
                    damage,
                    DamageType.DAMAGE_TYPE_MAGICAL,
                    DamageSource.DAMAGE_SOURCE_SPELLAOE,
                    false
                );

                AddBuff(
    "GragasQSlow",
    2f,
    (byte)1,          // ✅ stacks MUST be byte
    spell,            // originspell
    enemy,            // onto
    owner,            // from (ObjAIBase)
    false

      );
            }

            // allow recast again
            owner.SetSpell("GragasQ", 0, true);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnSpellCast.RemoveListener(this);
        }
    }
}
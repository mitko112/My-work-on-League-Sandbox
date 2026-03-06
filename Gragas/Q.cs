using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeaguePackets.Game.Common.CastInfo;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    
    public class GragasQ : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true
        };
       
        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var pos = new Vector2(
                spell.CastInfo.TargetPositionEnd.X,
                spell.CastInfo.TargetPositionEnd.Z
            );

            SpellCast(owner, 0, SpellSlotType.ExtraSlots, pos, pos, false, Vector2.Zero);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true);
        }
    }
    
    public class GragasQMissile : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new()
        {
            MissileParameters = new MissileParameters { Type = MissileType.Circle }
        };

        public static Vector2 spellpos;
        Spell storedSpell;
        ObjAIBase Owner;

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Owner = owner;
            storedSpell = spell;
            spellpos = end;

            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });

            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }

        private void OnMissileEnd(SpellMissile missile)
        {
            if (Owner == null)
                return;

            Owner.SetSpell("GragasQToggle", 0, true);


        }


    }

    public class GragasQToggle : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true
        };

        ObjAIBase Owner;
        Spell Spell;
        Particle p1;
        Particle p2;

        float fermentTime;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Owner = owner;
            Spell = spell;
            fermentTime = 0f;

            p1 = AddParticle(owner, null, "Gragas_Base_Q_Enemy.troy", GragasQMissile.spellpos, lifetime: 4f);
            p2 = AddParticle(owner, null, "Gragas_Base_Q_Ally.troy", GragasQMissile.spellpos, lifetime: 4f);
            
            AddBuff("GragasQ", 4f, 1, spell, owner, owner, false);
        }

        public void OnSpellCast(Spell spell)
        {
            Explode();
        }

        public void OnUpdate(float diff)
        {
            fermentTime += diff;
            if (fermentTime >= 4000f)
            {
                Explode();
            }
        }

        private void Explode()
        {
            RemoveParticle(p1);
            RemoveParticle(p2);

            float t = System.MathF.Min(fermentTime, 2000f) / 2000f;
            float multiplier = 1.0f + 0.5f * t; // 100% → 150%

            int lvl = Spell.CastInfo.SpellLevel;

            float baseDamage =
                new float[] { 80, 120, 160, 200, 240 }[lvl - 1]
                + Owner.Stats.AbilityPower.Total * 0.6f;

            float damage = baseDamage * multiplier;

            float baseSlow =
                new float[] { 0.30f, 0.35f, 0.40f, 0.45f, 0.50f }[lvl - 1];

            float slowAmount = baseSlow * multiplier;

            AddParticle(Owner, null, "gragas_barrelboom.troy", GragasQMissile.spellpos, lifetime: 4f);

            var enemies = GetUnitsInRange(GragasQMissile.spellpos, 250f, true);

            foreach (var enemy in enemies)
            {
                enemy.TakeDamage(
                    Owner,
                    damage,
                    DamageType.DAMAGE_TYPE_MAGICAL,
                    DamageSource.DAMAGE_SOURCE_SPELLAOE,
                    false
                    
                );

                AddBuff(
                    "GragasQSlow",
                    2f,
                    1,
                    Spell,
                    enemy,
                    Owner,
                    false
                );
            }

            Owner.SetSpell("GragasQ", 0, true);
            SealSpellSlot(Owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
            Owner.RemoveBuffsWithName("GragasQ");
        }
    }
}
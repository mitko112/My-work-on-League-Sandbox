using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;


namespace Spells
{
    public class GragasQ : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true,


        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public SpellSector DamageSector;

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {

        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var targetPos = GetPointFromUnit(owner, 850.0f);

            var spellpos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            SpellCast(owner, 0, SpellSlotType.ExtraSlots, spellpos, spellpos, false, Vector2.Zero);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true);

        }

        public void OnSpellChannel(Spell spell)
        {
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
        }
        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }

    public class GragasQMissile : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            //ApiEventManager.OnSpellMissileEnd.AddListener(this, p, CastKeg, false);
            //ApiEventManager.OnCreateSector.AddListener(owner, spell.CastInfo.Owner, CastKeg);
            //ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public SpellSector DamageSector;
        ObjAIBase Owner;
        Spell daspell;
        AttackableUnit datarget;
        public static Particle p1;
        public static Particle p2;
        public static Vector2 spellpos;

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Owner = owner;
            daspell = spell;
            datarget = target;
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });

            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, CastKeg, true);
        }

        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {


        }

        public void CastKeg(SpellMissile missile)
        {
            var owner = daspell.CastInfo.Owner;
            var targetPos = GetPointFromUnit(owner, 850.0f);
            spellpos = new Vector2(daspell.CastInfo.TargetPositionEnd.X, daspell.CastInfo.TargetPositionEnd.Z);
            //AddParticle(owner, null, "Gragas_Base_Q_Mis.troy", spellpos, lifetime: 0.5f , reqVision: false);
            ApiEventManager.OnSpellHit.AddListener(this, daspell, TargetExecute, true);
            owner.SetSpell("GragasQToggle", 0, true);

        }

        public void OnSpellChannel(Spell spell)
        {
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {






        }
        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }


    public class GragasQToggle : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true,
        };
        Particle p1;
        Particle p2;
        ObjAIBase Owner;
        AttackableUnit Target;
        Vector2 possition2;
        float ticks;
        float Damage;
        Spell Spell;

        public SpellSector DamageSector;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Spell = spell;
            p1 = AddParticle(owner, null, "Gragas_Base_Q_Enemy.troy", GragasQMissile.spellpos, lifetime: 4f);
            p2 = AddParticle(owner, null, "Gragas_Base_Q_Ally.troy", GragasQMissile.spellpos, lifetime: 4f);
            Owner = owner;
            AddBuff("GragasQ", 4f, 1, spell, owner, owner);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            //var spellpos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            Target = target;
            DamageSector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 250f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = false,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
                Lifetime = 2f
            });
        }

        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            //var targetPos = possition2
            var position = GragasQMissile.spellpos;
            owner.SetSpell("GragasQ", 0, true);
            AddParticle(owner, null, "gragas_barrelboom.troy", position, lifetime: 4f);
            RemoveParticle(p1);
            RemoveParticle(p2);

            var ap = owner.Stats.AbilityPower.Total * 0.3f;
            var damage = (40 * (spell.CastInfo.SpellLevel)) + ap;
            Damage = damage;
            var enemies = GetUnitsInRange(position, 250f, true);
            foreach (var enemy in enemies)
            {
                enemy.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            }
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
            owner.RemoveBuffsWithName("GragasQ");

        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {

        }

        public void OnSpellPostCast(Spell spell)
        {
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            ticks += diff;
            if (ticks > 4000)
            {
                AddParticle(Owner, null, "gragas_barrelboom.troy", GragasQMissile.spellpos, lifetime: 4f);
                var enemies = GetUnitsInRange(GragasQMissile.spellpos, 250f, true);
                var ap = Owner.Stats.AbilityPower.Total * 0.3f;
                var damage = (40 * (Spell.CastInfo.SpellLevel)) + ap;
                foreach (var enemy in enemies)
                {
                    enemy.TakeDamage(Spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                }
                //Owner.SetSpell("GragasQ", 0, true);
                ticks = 0;
            }
        }
    }
}


using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class XenZhaoParry : ISpellScript
    {
        Spell Spell;
        public byte HitChampion;
        public SpellScriptMetadata ScriptMetadata => new()
        {
            IsDamagingSpell = true,
            TriggersSpellCasts = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, OnSpellHit, false);
        }

        public void OnSpellPostCast(Spell spell)



        {
            Spell = spell;
            HitChampion = 0;
            Spell.CreateSpellSector(new SectorParameters
            {
                Length = 450f,
                SingleTick = true,
                Type = SectorType.Area,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes
            });
        }
        public void OnSpellHit(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = Spell.CastInfo.Owner;
            float AD = owner.Stats.AttackDamage.FlatBonus;
            float Damage = 75 * Spell.CastInfo.SpellLevel + AD;
            if (!(target.Team == owner.Team || target is BaseTurret || target is ObjBuilding))
            {
                FaceDirection(owner.Position, target);
                Damage += target.Stats.CurrentHealth * 0.15f;
                if ((target is Monster || target is Minion) && Damage > 600f)
                {
                    Damage = 600f;
                }
                AddParticleTarget(owner, target, "xenZiou_utl_tar.troy", target, 1f, 1f);
                AddParticleTarget(owner, target, "xenZiou_utl_tar_02.troy", target, 1f, 1f);
                AddParticleTarget(owner, target, "xenZiou_utl_tar_03.troy", target, 1f, 1f);

                target.TakeDamage(owner, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                    ForceMovement(target, null, GetPointFromUnit(target, -(450.0f)), 1800, 0, 80, 0);
                }
                if (target is Champion)
                {
                    HitChampion += 1;
                    AddBuff("XenZhaoParry", 6.0f, 1, Spell, owner, owner);
                }
            }
        }
    }

    
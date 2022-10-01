using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;

using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;

namespace Spells
{
    public class CassiopeiaMiasma : ISpellScript
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



        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var targetPos = GetPointFromUnit(owner, 850.0f);
            SpellCast(owner, 1, SpellSlotType.ExtraSlots, targetPos, targetPos, false, Vector2.Zero);
            var spellpos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);


            AddParticle(owner, null, "Cassiopeia_Base_W_Miasma_cas.troy", spellpos, lifetime: 7f);
            AddParticle(owner, null, "Cassiopeia_Base_W_Miasma_mis.troy", spellpos, lifetime: 7f);
            AddParticle(owner, null, "Cassiopeia_Base_W_Miasma_tar_green.troy", spellpos, lifetime: 7f);
            AddParticle(owner, null, "Cassiopeia_Base_W_Miasma_tar_red.troy", spellpos, lifetime: 7f);
            DamageSector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 250f,
                Tickrate = 2,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area,
                Lifetime = 7f
            });

        }

        public void OnSpellChannel(Spell spell)
        {
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("CassiopeiaPoisonTicker2", 2f, 1, spell, target, owner);






        }
    }
}


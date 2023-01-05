using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class AkaliSmokeBomb : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };
        
        public SpellSector SpellSector;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
            

        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }
        
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            

            SpellSector = spell.CreateSpellSector(new SectorParameters
            {
                Tickrate = 1,
                Length = 300f,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes, 
                Lifetime = 8f,
                Type = SectorType.Area,
                CanHitSameTargetConsecutively = true,
            }) ;
        }
         
        public void OnSpellCast(Spell spell)
        {
        }

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var spellPos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            AddParticle(owner, null, "akali_smoke_bomb_tar.troy", spellPos, 8f);
            AddParticle(owner, null, "akali_smoke_bomb_tar_team_green.troy", spellPos, 8f);
            AddBuff("AkaliWStealth", 8f, 1, spell, owner, owner, false);

            /*
              TODO: Display green border (akali_smoke_bomb_tar_team_green.troy) for the own team,
              display red border (akali_smoke_bomb_tar_team_red.troy) for the enemy team
              Currently only displaying the green border for everyone.
              -Add invisibility.
            */
        }
        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("AkaliWDebuff", 1f, 1, spell, target, owner, false);

            
            
        }
        

        }
        }
        


        




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
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class JaxRelentlessAssault : ISpellScript
    {
        ObjAIBase Owner;
        Spell Spell;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            
            Spell = spell;
            Owner = spell.CastInfo.Owner;
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, true);
             AddBuff("JaxRelentlessAttack", 8f, 1, spell, owner, owner, true);

        }
        public void OnLevelUp(Spell spell)

        {
            
             ApiEventManager.OnHitUnit.AddListener(this, spell.CastInfo.Owner, OnHitUnit, false);
            }

        public void OnHitUnit(DamageData damageData)
        {
            var target = damageData.Target;
           AddBuff("JaxRelentlessAttack", 2.5f, 1, Spell, target, Owner, false);
        }
        
    
        

        public void OnSpellCast(Spell spell)
        {
            var Owner = spell.CastInfo.Owner;

            AddBuff("JaxRelentlessAssaultAS", 8f, 1, spell, Owner, Owner, false);
            AddParticleTarget(Owner, Owner, "JaxRelentlessAssault_buf.troy", Owner, 8f, 1f);
            
        }

        }
    }

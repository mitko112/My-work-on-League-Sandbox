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
    public class PoppyParagonOfDemacia : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true,


        };
        ObjAIBase daowner;
        Spell daspell;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            daowner = owner;
            daspell = spell;
            {
                ApiEventManager.OnHitUnit.AddListener(this, owner, GivePoppySomething, false);
            }
        }
        public void GivePoppySomething(DamageData damageData)
        {
            AddBuff("PoppyParagonManager", 5f, 1, daspell, daowner, daowner, false);
        }

    

        public SpellSector DamageSector;

        
        public void AddPoppyWPassive(Spell spell)
        {
            AddBuff("PoppyParagonManager", 5f, 1, spell, daowner, daowner, false);
        }

        public void OnSpellCast(Spell spell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this, daowner);
            var owner = spell.CastInfo.Owner;
            AddBuff("PoppyParagonSpeed", 5f, 1, spell, owner, owner);
            //owner.RemoveBuffsWithName("PoppyParagonManager");
            for (int i = 1; i <= 10; i++)
            {
                AddBuff("PoppyParagonManager", 5f, 1, spell, daowner, daowner, false);

            }
        }

        public void OnSpellPostCast(Spell spell)
        {
            ApiEventManager.OnHitUnit.AddListener(this, daowner, GivePoppySomething, false);

        }

        
    }
}
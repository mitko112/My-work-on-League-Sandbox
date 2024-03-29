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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class RenektonCleave : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };


        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            var ad = spell.CastInfo.Owner.Stats.AttackDamage.FlatBonus * 0.8f;
            var oli = 60 * spell.CastInfo.SpellLevel + ad;
            var damage = oli;
            var maxchamp = 50 * spell.CastInfo.SpellLevel;
            var damagefury = oli +oli* 0.5f;
            var monsterheal = oli * 0.05f;
            var heal = oli * 0.2f;
                var heali = oli*0.4f;
            //if (damage > 250)
            //{
            //owner.Stats.CurrentHealth += maxchamp;
            //}
            //else
            //{
            owner.Stats.CurrentHealth += heal;
            //}
            if (owner.Stats.CurrentMana > 50)
            {
                owner.Stats.CurrentHealth += heali;
            }



                AddParticle(owner, null, "RenektonCleave_trail.troy", owner.Position, direction: owner.Direction);
            PlayAnimation(owner, "Spell1", 0.7f);


            var units = GetUnitsInRange(owner.Position, 425f, true);

            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == owner.Team || units[i] is BaseTurret || units[i] is ObjBuilding || units[i] is Inhibitor))

                    
                {
                    
                   

                    if (owner.Stats.CurrentMana > 50)
                    {
                        var mana = 50;
                        owner.Stats.CurrentMana -= mana;
                        units[i].TakeDamage(owner, damagefury, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);

                    }
                    else
                        units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);

                }
            }

        }

        
    }

}
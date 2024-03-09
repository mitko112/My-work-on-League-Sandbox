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
    public class RenektonSliceAndDice : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true
            // TODO
        };


        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var Adratio = owner.Stats.AttackDamage.FlatBonus * 0.9f;
            var damage = 40 * (spell.CastInfo.SpellLevel) + Adratio;
            var targetPos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            var damagefury = damage +damage*0.5f;
            FaceDirection(targetPos, owner);
            ForceMovement(owner, "Spell3", targetPos, 1200, 0, 0, 0);
            var units = GetUnitsInRange(owner.Position, 750f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == owner.Team || units[i] is BaseTurret || units[i] is ObjBuilding || units[i] is Inhibitor))
                {
                    



                    
                    AddBuff("RenektonSliceAndDice", 4.0f, 1, spell, owner, owner);
                    if (owner.Stats.CurrentMana > 50)
                    {
                        var mana = 50;
                        owner.Stats.CurrentMana -= mana;
                        units[i].TakeDamage(owner, damagefury, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                        AddBuff("RenektonSliceAndDice", 4.0f, 1, spell, owner, owner);
                        AddBuff("ArmorReduction", 4.0f, 1, spell, units[i], owner);
                        
                    }
                    else

                    units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);

                }
            }
        }

    }
}
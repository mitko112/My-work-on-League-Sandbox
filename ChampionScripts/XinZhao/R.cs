using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using GameServerCore.Enums;

namespace Spells
{
    public class XenZhaoParry : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };


        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner as Champion;
            
            var ad = spell.CastInfo.Owner.Stats.AttackDamage.FlatBonus*2f;
            
            var damage = 75*spell.CastInfo.SpellLevel + ad;
            var trueCoords = GetPointFromUnit(owner, 500f);




            var units = GetUnitsInRange(owner.Position, 187f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == owner.Team || units[i] is BaseTurret || units[i] is ObjBuilding || units[i] is Inhibitor))
                {
                    units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                     ForceMovement(units[i], "RUN", trueCoords, 2200, 0, 0, 0);
                }
            }

        }

        
    }

}
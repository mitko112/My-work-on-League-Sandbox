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
using System.Numerics;
using GameServerLib.GameObjects.AttackableUnits;

namespace Spells

    {
    
    public class VayneCondemn : ISpellScript
    {
      

        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
 
            // TODO
        };

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            SpellCast(owner, 1, SpellSlotType.ExtraSlots, false, target, Vector2.Zero);

        }


    }
    public class VayneCondemnMissile: ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            // TODO
        };
        ObjAIBase Owner;
       
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Owner = owner;
            
        }
        public void OnSpellPostCast(Spell spell)
        {
            var target = spell.CastInfo.Targets[0].Unit;
            FaceDirection(Owner.Position, target);
            var ADratio = Owner.Stats.AttackDamage.FlatBonus * 0.5f;
            var damage = 45 * spell.CastInfo.SpellLevel + ADratio;
            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            ForceMovement(target, "RUN", GetPointFromUnit(target, -(470)), 2200, 0, 0, 0);
            
        }
    }
}


        


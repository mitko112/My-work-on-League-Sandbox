using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class DariusAxeGrabCone: ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true
            // TODO
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnLevelUpSpell.AddListener(owner, spell, OnLevelUp, false);
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);

        }

        public void OnLevelUp(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("DariusPassiveArmorPen", 1, 1, spell, owner, owner, true);
        }
        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;

            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            FaceDirection(spellPos, owner, false);

            var sector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 540f,
                SingleTick = true,
                ConeAngle = 24.76f,
                Type = SectorType.Cone
            });

            
        }

        public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
        {
            var owner = spell.CastInfo.Owner;

            ForceMovement(target, "RUN", owner.Position, 540f, 0, 5.0f, 0, movementOrdersType: ForceMovementOrdersType.CANCEL_ORDER);
        }

    }
}

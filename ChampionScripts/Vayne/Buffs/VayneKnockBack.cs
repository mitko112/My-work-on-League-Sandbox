using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using System.Numerics;
using GameServerLib.GameObjects.AttackableUnits;


namespace Buffs
{
    internal class VayneKnockBack : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {

                buff.SetStatusEffect(StatusFlags.CanAttack | StatusFlags.CanMove | StatusFlags.CanCast, false);
            ForceMovement(unit, "RUN", GetPointFromUnit(unit, -(470)), 1800f, 0, 0.1f, 0, movementOrdersType: ForceMovementOrdersType.CANCEL_ORDER);
        }

        // TODO: Use OnMoveEnd event and call this manually.
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (buff.SourceUnit is ObjAIBase ai && unit is Champion ch)
            {
                ai.SetTargetUnit(ch, true);
            }
        }
    }
}
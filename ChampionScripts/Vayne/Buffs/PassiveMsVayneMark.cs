using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using Newtonsoft.Json.Schema;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Buffs
{
    internal class PassiveMSVayneMark: IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.HASTE,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();


        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (unit.HasBuff("VayneInquisition"))
            {

                StatsModifier.MoveSpeed.FlatBonus += 60;
                unit.AddStatModifier(StatsModifier);
            }

            else
            {
                StatsModifier.MoveSpeed.FlatBonus += 30;
                unit.AddStatModifier(StatsModifier);
            }
            

        }



    }
}
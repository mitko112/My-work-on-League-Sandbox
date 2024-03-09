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
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;


namespace Buffs
{
    internal class IreliaTranscendentBlades : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };
        

        public StatsModifier StatsModifier { get; private set; }

        int counter = 0;
        ObjAIBase Unit;
        Buff Buff;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Buff = buff;
            if (unit is ObjAIBase obj)
            {
                Unit = obj;
                obj.SetSpell("IreliaTranscendentBladesSpell", 3, true);
                ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("IreliaTranscendentBladesSpell"), OnSpellCast);
            }
        }

        public void OnSpellCast(Spell spell)
        {
            counter++;
            if (counter >= 4 && (Unit).Spells[3].SpellName == "IreliaTranscendentBladesSpell")
            {
                Unit.SetSpell("IreliaTranscendentBlades", 3, true);
                Buff.DeactivateBuff();
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnSpellCast.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
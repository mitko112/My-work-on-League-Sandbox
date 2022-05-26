using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class IreliaTranscendentBlades : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };
        

        public IStatsModifier StatsModifier { get; private set; }

        int counter = 0;
        IObjAiBase Unit;
        IBuff Buff;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Buff = buff;
            if (unit is IObjAiBase obj)
            {
                Unit = obj;
                obj.SetSpell("IreliaTranscendentBladesSpell", 3, true);
                ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("IreliaTranscendentBladesSpell"), OnSpellCast);
            }
        }

        public void OnSpellCast(ISpell spell)
        {
            counter++;
            if (counter >= 4 && (Unit).Spells[3].SpellName == "IreliaTranscendentBladesSpell")
            {
                Unit.SetSpell("IreliaTranscendentBlades", 3, true);
                Buff.DeactivateBuff();
            }
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnSpellCast.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
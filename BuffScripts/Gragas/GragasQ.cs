using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;

namespace Buffs
{
    class GragasQ : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Spell ThisSpell;
        Buff ThisBuff;
        Minion Shadow;
        Buff ShadowBuff;
        public bool QueueSwap;
        bool hascasted = false;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ThisSpell = ownerSpell;
            ThisBuff = buff;

            ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("GragasQ"), TargetExecute);
        }
        public void TargetExecute(Spell spell)
        {
            hascasted = true;
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (hascasted == false) ownerSpell.CastInfo.Owner.SetSpell("GragasQ", 0, true);
            SealSpellSlot(ownerSpell.CastInfo.Owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
        }



        public void OnUpdate(float diff)
        {
        }
    }
}

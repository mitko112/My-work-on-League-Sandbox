using System.Numerics;
using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    class GragasQ : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };
        

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        ISpell ThisSpell;
        IBuff ThisBuff;
        IMinion Shadow;
        IBuff ShadowBuff;
        public bool QueueSwap;
        bool hascasted = false;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ThisSpell = ownerSpell;
            ThisBuff = buff;

            ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell(0), TargetExecute);
        }
        public void TargetExecute(ISpell spell) 
        { 
            hascasted = true;
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            if (hascasted == false) ownerSpell.CastInfo.Owner.SetSpell("GragasQ", 0, true);
            SealSpellSlot(ownerSpell.CastInfo.Owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
        }

        

        public void OnUpdate(float diff)
        {
        }
    }
}
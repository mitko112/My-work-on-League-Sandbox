using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;

using GameServerCore.Scripting.CSharp;

using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Buffs

{
    internal class FizzPassive : IBuffGameScript


    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        Spell Spell;
        AttackableUnit Unit;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();


        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)



        {
            Spell = ownerSpell;
            ApiEventManager.OnTakeDamage.AddListener(this, unit, OnTakeDamage, false);
            Unit = unit;
        }


        public void OnTakeDamage(DamageData data)


        {

            {
                var owner = Spell.CastInfo.Owner;
                var reductionbylevel = owner.GetSpell("CharScriptFizz").CastInfo.SpellLevel;
                data.Damage = -4 * reductionbylevel;

                Unit.SetStatus(StatusFlags.Ghosted, true);

            }
        }

    }
}

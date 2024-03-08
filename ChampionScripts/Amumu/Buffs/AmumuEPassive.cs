using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;

using GameServerCore.Scripting.CSharp;

using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;

namespace Buffs

{
    internal class AmumumuEPassive : IBuffGameScript

       
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        Spell Spell;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)

            
            
        {
            Spell = ownerSpell;
            ApiEventManager.OnTakeDamage.AddListener(this, unit, OnTakeDamage, false);

        } 
         
        
        public void OnTakeDamage(DamageData data)

            
        {
            if (data.IsAutoAttack)
            {
                var owner = Spell.CastInfo.Owner;
                var reductionbylevel = owner.GetSpell("Tantrum").CastInfo.SpellLevel;
                data.Damage = -2* reductionbylevel;

                owner.GetSpell("Tantrum").LowerCooldown(0.5f); 

            }
        }
        
    }
}

   



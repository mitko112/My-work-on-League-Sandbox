using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace Buffs
{
    public class MalphiteGraniteShield : IBuffGameScript
    {
        ObjAIBase _owner;
        Buff thisBuff;
        float _maxAbsorb;

        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            _owner = unit as ObjAIBase;

            _maxAbsorb = _owner.Stats.HealthPoints.Total * 0.10f;

            ApiEventManager.OnTakeDamage.AddListener(this, _owner, OnTakeDamage);
            
        }

        void OnTakeDamage(DamageData damageData)
        {
            if (damageData.Target != _owner)
                return;

            if (damageData.PostMitigationDamage > _maxAbsorb)
                damageData.PostMitigationDamage -= _maxAbsorb;
            else
                damageData.PostMitigationDamage = 0f;

            // shield is consumed
            RemoveBuff(thisBuff);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnTakeDamage.RemoveListener(this);
        }
    }
}

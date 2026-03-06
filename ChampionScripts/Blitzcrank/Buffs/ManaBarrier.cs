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
    public class BlitzcrankManaBarrier : IBuffGameScript
    {
        private ObjAIBase _owner;
        private Particle _barrierParticle;
        private float _maxAbsorb;


        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        Buff thisBuff;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            _owner = unit as ObjAIBase;

            _maxAbsorb = _owner.Stats.CurrentMana * 0.10f;

            // ✅ Shield particle
            _barrierParticle = AddParticleTarget(
                _owner,
                _owner,
                "SteamGolemShield.troy",
                _owner
            );

            ApiEventManager.OnTakeDamage.AddListener(this, _owner, OnTakeDamage);
        }

            

            private void OnTakeDamage(DamageData damageData)
        {
            if (damageData.Target != _owner)
                return;

            // Reduce damage once
            if (damageData.PostMitigationDamage > _maxAbsorb)
                damageData.PostMitigationDamage -= _maxAbsorb;
            else
                damageData.PostMitigationDamage = 0f;

            // Consume barrier
            RemoveBuff(thisBuff);
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnTakeDamage.RemoveListener(this);

            if (_barrierParticle != null)
                RemoveParticle(_barrierParticle);
        }
    }
}
    
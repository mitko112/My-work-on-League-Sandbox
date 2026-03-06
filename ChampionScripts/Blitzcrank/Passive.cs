using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace CharScripts
{
    public class CharScriptBlitzcrank : ICharScript
    {
        private ObjAIBase _owner;
        private bool _onCooldown;
        private bool _barrierActive;
        private float _cooldownTimer;

        private const float COOLDOWN = 90f;
        private const float HP_THRESHOLD = 0.20f;

        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            _owner = owner;
            _onCooldown = false;
            _barrierActive = false;

            ApiEventManager.OnTakeDamage.AddListener(this, owner, OnTakeDamage);
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell = null)
        {
            ApiEventManager.OnTakeDamage.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
            if (_onCooldown)
            {
                _cooldownTimer -= diff;
                if (_cooldownTimer <= 0f)
                    _onCooldown = false;
            }
        }

        private void OnTakeDamage(DamageData damageData)
        {
            if (damageData.Target != _owner)
                return;

            // Barrier already active → damage reduction handled in buff
            if (_barrierActive)
                return;

            if (_onCooldown)
                return;

            float hpPercent =
                _owner.Stats.CurrentHealth / _owner.Stats.HealthPoints.Total;

            if (hpPercent > HP_THRESHOLD)
                return;

            ActivateManaBarrier();
        }

        private void ActivateManaBarrier()
        {
            _barrierActive = true;
            _onCooldown = true;
            _cooldownTimer = COOLDOWN;

            // Buff is visual + state holder
            AddBuff(
                "BlitzcrankManaBarrier",
                10.0f,
                1,
                null,
                _owner,
                _owner
            );
        }

        public void ConsumeBarrier()
        {
            _barrierActive = false;
        }

        public bool IsBarrierActive()
        {
            return _barrierActive;
        }
    }
}

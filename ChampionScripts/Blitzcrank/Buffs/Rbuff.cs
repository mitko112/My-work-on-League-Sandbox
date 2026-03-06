using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Linq;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class BlitzcrankRPulse : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; } = new()
        {
            BuffType = BuffType.AURA,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 3,
            IsHidden = true
        };

        private ObjAIBase _owner;
        private Buff _buff;

        private float _timer;
        private const float INTERVAL = 1.0f;

        public StatsModifier StatsModifier { get; } = new();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            _owner = unit as ObjAIBase;
            _buff = buff;
            _timer = 0f;

            // ✅ THIS is the missing piece
            
        }

        private void OnTick(float diff)
        {
            if (_owner == null || _owner.IsDead)
                return;

            if (_buff == null || _buff.StackCount <= 0)
                return;

            _timer += diff;
            if (_timer < INTERVAL)
                return;

            _timer = 0f;

            var target = GetUnitsInRange(_owner.Position, 475f, true)
                .Where(u =>
                    u.Team != _owner.Team &&
                    !u.IsDead &&
                    u is ObjAIBase)
                .OrderBy(u => Vector2.Distance(_owner.Position, u.Position))
                .FirstOrDefault();

            if (target == null)
                return;

            int rLevel = _buff.StackCount;
            float damage = 40 + (40 * rLevel); // 80 / 120 / 160

            target.TakeDamage(
                _owner,
                damage,
                DamageType.DAMAGE_TYPE_MAGICAL,
                DamageSource.DAMAGE_SOURCE_SPELL,
                false
            );

            AddParticleTarget(_owner, target, "Blitzcrank_R_Passive.troy", target);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
           
        }
    }
}

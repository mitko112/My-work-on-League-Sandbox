using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class NasusW : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.SLOW,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private StatsModifier _modifier;
        private AttackableUnit _target;

        private float _elapsed;
        private float _duration;

        private const float START_SLOW = 0.35f;
        private float _endMoveSlow;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            _target = unit;
            _elapsed = 0f;
            _duration = buff.Duration; // seconds

            int level = ownerSpell.CastInfo.SpellLevel;
            _endMoveSlow = new float[]
            {
                0f, 0.47f, 0.59f, 0.71f, 0.83f, 0.95f
            }[level];

            _modifier = new StatsModifier();

            AddParticleTarget(
                ownerSpell.CastInfo.Owner,
                unit,
                "Global_Slow.troy",
                unit,
                buff.Duration
            );
        }

        public void OnUpdate(float diff)
        {
            if (_target == null)
                return;

            // 🔴 diff is MILLISECONDS on your LS
            _elapsed += diff / 1000f;

            float t = _elapsed / _duration;
            if (t > 1f)
                t = 1f;

            float moveSlow =
                START_SLOW + (_endMoveSlow - START_SLOW) * t;

            // Force stat refresh (old LS way)
            _target.RemoveStatModifier(_modifier);

            _modifier = new StatsModifier();
            _modifier.MoveSpeed.PercentBonus = -moveSlow;
            _modifier.AttackSpeed.PercentBonus = -(moveSlow * 0.5f);

            _target.AddStatModifier(_modifier);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            unit.RemoveStatModifier(_modifier);
        }
    }
}
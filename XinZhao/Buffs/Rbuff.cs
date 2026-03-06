using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Buffs
{
    internal class XenZhaoParry : IBuffGameScript
    {
        Particle _armorShield;
        Particle _magicShield;
        ObjAIBase _unit;
        Spell Spell;
        private const float ArmorMrPerStack = 15f;
        public BuffScriptMetaData BuffMetaData { get; } = new()
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 5
        };
        public StatsModifier StatsModifier { get; } = new();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (unit is not ObjAIBase ai)
                return;

            _unit = ai;

            UpdateStats(buff);

            _armorShield = AddParticleTarget(
                _unit, _unit,
                "xenziou_selfshield_01.troy",
                _unit,
                bone: "C_BuffBone_Glb_Center_Loc"
            );

            _magicShield = AddParticleTarget(
                _unit, _unit,
                "xenziou_selfshield_01_magic.troy",
                _unit
            );
        }

        public void OnStack(AttackableUnit unit, Buff buff)
        {
            UpdateStats(buff);
        }

        private void UpdateStats(Buff buff)
        {
            // Remove old values
            _unit.RemoveStatModifier(StatsModifier);

            float totalBonus = ArmorMrPerStack * buff.StackCount;

            StatsModifier.Armor.FlatBonus = totalBonus;
            StatsModifier.MagicResist.FlatBonus = totalBonus;

            _unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(_armorShield);
            RemoveParticle(_magicShield);

            unit.RemoveStatModifier(StatsModifier);

            // Safety reset to avoid reuse bugs
            StatsModifier.Armor.FlatBonus = 0;
            StatsModifier.MagicResist.FlatBonus = 0;
        }

        public void OnUpdate(float diff) { }
    }
}
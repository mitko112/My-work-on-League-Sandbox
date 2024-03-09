
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
        Particle ArmorShield;
        Particle MagicShield;
        ObjAIBase Unit;
        public BuffScriptMetaData BuffMetaData { get; } = new()
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.RENEW_EXISTING
        };
        public StatsModifier StatsModifier { get; protected set; } = new();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if (unit is ObjAIBase ai)
            {
                Unit = ai;

                if (ownerSpell.Script is Spells.XenZhaoParry script)
                {
                    float HitChampion = script.HitChampion;
                    float Mark = 10 + 5 * Unit.GetSpell("XenZhaoParry").CastInfo.SpellLevel;
                    StatsModifier.Armor.FlatBonus += Mark * HitChampion;
                    StatsModifier.MagicResist.FlatBonus += Mark * HitChampion;
                    unit.AddStatModifier(StatsModifier);
                }
                ArmorShield = AddParticleTarget(Unit, Unit, "xenziou_selfshield_01.troy", Unit, bone: "C_BuffBone_Glb_Center_Loc");
                MagicShield = AddParticleTarget(Unit, Unit, "xenziou_selfshield_01_magic.troy", Unit);
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(ArmorShield);
            RemoveParticle(MagicShield);
        }
    }
}


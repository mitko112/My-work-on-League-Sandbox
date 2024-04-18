using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Collections.Generic;
using System.Linq;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class GarenW : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private string[] particleNames =
        {
            "Garen_Base_W_Avatar.troy",
            "Garen_Base_W_Buff.troy",
            "Garen_Base_W_Cas.troy",
            "Garen_Base_W_Shoulder_L.troy",
            "Garen_Base_W_Shoulder_R.troy"
        };
        private List<Particle> particles = new List<Particle>();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;

            StatsModifier.Tenacity.PercentBonus += 30;
            unit.AddStatModifier(StatsModifier);

            particleNames.ToList().ForEach(particleNames =>
            {
                Particle particle = AddParticleTarget(unit, unit, particleNames, unit, buff.Duration);
                particles.Add(particle);
            });

            ApiEventManager.OnPreTakeDamage.AddListener(this, unit, PreTakeDamage, false);
        }

        public void PreTakeDamage(DamageData dmg)
        {
            dmg.PostMitigationDamage *= 0.7f;
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ApiEventManager.OnPreTakeDamage.RemoveListener(this);
            particles.ForEach(p =>
            {
                RemoveParticle(p);
            });
            AddParticleTarget(unit, unit, "Garen_Base_W_Avatar_Fade.troy", unit);
        }
    }
}

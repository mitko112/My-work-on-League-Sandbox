using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using            GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using System.Linq;
using LeagueSandbox.GameServer.Logging;
using CharScripts;

namespace Buffs
{
    internal class GarenPassiveCooldown : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public StatsModifier StatsModifier { get; private set; }

        ObjAIBase unit;
        Spell spell;
        Buff thisBuff;

        private bool resetting = false;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            this.spell = ownerSpell;
            this.thisBuff = buff;
            this.unit = ownerSpell.CastInfo.Owner;

            ApiEventManager.OnTakeDamage.AddListener(this, unit, OnTakeDamage);
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            if(!resetting)
            {
                AddBuff("GarenPassiveHeal", 99999f, 1, ownerSpell, unit, ownerSpell.CastInfo.Owner);
            } else
            {
                AddBuff("GarenPassiveCooldown", CharScriptGaren.GetCooldownForLevel(unit.Stats.Level), 1, spell, unit, ownerSpell.CastInfo.Owner);
            }
            ApiEventManager.OnTakeDamage.RemoveListener(this);
        }

        public void OnTakeDamage(DamageData data)
        {
            if(!CharScriptGaren.ShouldPassiveTurnOff(unit, data))
            {
                return;
            }

            resetting = true;
            RemoveBuff(thisBuff);
        }
    }
}

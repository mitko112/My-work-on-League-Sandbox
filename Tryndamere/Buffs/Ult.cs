using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;
using GameServerLib.GameObjects.AttackableUnits;
using System.Security.Cryptography.X509Certificates;
using LeaguePackets.Game;

namespace Buffs
{
    internal class UndyingRage : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        AttackableUnit Unit;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Unit = unit;
            ApiEventManager.OnTakeDamage.AddListener(this, unit, OnTakeDamage, false);

        }
        public void OnTakeDamage(DamageData data)
        {
            if (data.Damage > Unit.Stats.CurrentHealth)
            {
                Unit.SetStatus(StatusFlags.Invulnerable, true);
                Unit.Stats.CurrentHealth = 1;
            }
        }

           
            
        

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {




            unit.SetStatus(StatusFlags.Invulnerable, false);

        }

    }
}
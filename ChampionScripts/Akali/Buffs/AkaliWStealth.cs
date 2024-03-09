using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;
using static LeaguePackets.Game.Common.CastInfo;

namespace Buffs
{
    internal class AkaliWStealth : IBuffGameScript
    {
        public  BuffScriptMetaData BuffMetaData { get; } = new()
        {
            BuffType = BuffType.AURA,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1,
            IsHidden = false
        };
        public StatsModifier StatsModifier { get; protected set; } = new();

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)

        {
            StatsModifier.Armor.FlatBonus = 10;
            StatsModifier.MagicResist.FlatBonus = 10;
            unit.AddStatModifier(StatsModifier);

            unit.SetStatus(StatusFlags.Stealthed, true);
            
        }
            public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
            {
                unit.SetStatus(StatusFlags.Stealthed, false);
            }
        }

        }
    

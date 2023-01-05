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
using System.Buffers;

namespace Buffs
{
    internal class JaxESelfcast : IBuffGameScript
    {
        Vector2 spellpos;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        Spell Spell;
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();


        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
            
        {
            Spell = ownerSpell;
            var owner = Spell.CastInfo.Owner;
            SpellCast(owner, 3, SpellSlotType.ExtraSlots, spellpos, spellpos, false, Vector2.Zero);


        }
        }
    }

using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Collections.Generic;

namespace Buffs
{
    public class BrandWildfireTracker : IBuffGameScript
    {
        public float StoredDamage;
        public int MaxHits;
       
        public BuffScriptMetaData BuffMetaData { get; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 5
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

            

            public List<uint> HitTargets = new List<uint>();

            public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell) {
            
        }
            public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell) { }
            public void OnUpdate(float diff) { }
        }
    }
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


namespace Buffs
{
    class AkaliMota : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData 
        {
            BuffType = BuffType.COMBAT_DEHANCER
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Particle p;
        Particle p2;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "akali_markOftheAssasin_marker_tar", unit, buff.Duration);
            p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "akali_markOftheAssasin_marker_tar_02", unit, buff.Duration);
            //TODO: Find the overhead particle effects
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p);
            RemoveParticle(p2);
        }
    }
}


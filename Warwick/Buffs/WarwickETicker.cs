using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using Newtonsoft.Json.Schema;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using static LeaguePackets.Game.Common.CastInfo;

namespace Buffs
{
    internal class BloodScentTicker : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };


        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            
            //AddParticleTarget(owner, owner, "wolfman_bloodscent_activate_bloodless_buff.troy", owner, 1f, 1f);
            //(owner, owner, "wolfman_bloodscent_activate_blood_buff.troy", owner, 1f, 1f);
            //AddParticleTarget(owner, owner, "wolfman_bloodscent_activate_bloodless_buf_02.troy", owner, 1f, 1f);
            //AddParticleTarget(owner, owner, "wolfman_bloodscent_activate_blood_buff_02.troy", owner, 1f, 1f);
            //AddParticleTarget(owner, owner, "Bloodscentsniff_cas.troy", owner, 1f, 1f);
            AddParticleTarget(unit, unit, "wolfman_bloodscent_marker.troy", unit, 1f, 1f);
            AddParticleTarget(owner, owner, "wolfman_bloodscent_activate_speed.troy", owner, 1f, 1f);
            //AddParticleTarget(unit, unit, "wolfman_bloodscent_marker_goa_bloodless.troy", unit, 1f, 1f);

            AddUnitPerceptionBubble(unit, 1000, buff.Duration, owner.Team);
        }



    }
}


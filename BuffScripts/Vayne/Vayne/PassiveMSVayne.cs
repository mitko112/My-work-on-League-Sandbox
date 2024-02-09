using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.API;

namespace Buffs
{
    internal class PassiveMSVayne : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public SpellSector NightHunter;
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            
        NightHunter = ownerSpell.CreateSpellSector(new SectorParameters
            {

        BindObject = ownerSpell.CastInfo.Owner,
                Length = 2000f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
    });

            ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, TargetExecute, false);

    }
    public void TargetExecute(Spell spell, AttackableUnit target, SpellMissile missile, SpellSector sector)
    {

            var owner = spell.CastInfo.Owner;
            AddBuff("PassiveMSVayneMark", 1f, 1, spell, owner, owner, false);
        }

            public void OnUpdate(float diff)
        {
        }
    }
}
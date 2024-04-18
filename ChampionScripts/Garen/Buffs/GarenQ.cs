using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeaguePackets.Game.Events;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Logging;
using LeagueSandbox.GameServer.Scripting.CSharp;
using Spells;
using System;
using System.Numerics;
using System.Reflection;
using static LeaguePackets.Game.Common.CastInfo;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class GarenQ : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        Particle weaponParticle;

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private ObjAIBase owner;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            this.owner = ownerSpell.CastInfo.Owner;

            weaponParticle = AddParticleTarget(owner, owner, "Garen_Base_Q_Cas_Sword.troy", unit, lifetime: 4.5f, bone: "weapon");

            owner.CancelAutoAttack(true);
            owner.SkipNextAutoAttack();

            ApiEventManager.OnPreAttack.AddListener(this, owner, OverrideNextAutoAttack);
        }

        private void OverrideNextAutoAttack(Spell spell)
        {
            if(!"GarenQAttack".Equals(spell.SpellName))
            {
                owner.SetAutoAttackSpell("GarenQAttack", false);
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            ownerSpell.CastInfo.Owner.SetAutoAttackSpell(Spells.GarenQ.BASIC_AUTO_ATTACK_SPELL, false);

            SealSpellSlot(ownerSpell.CastInfo.Owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
            ownerSpell.SetCooldown(8);
            if (weaponParticle != null)
            {
                RemoveParticle(weaponParticle);
            }

            ApiEventManager.OnPreAttack.RemoveListener(this);
        }
    }
}

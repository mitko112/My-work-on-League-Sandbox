using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class GarenQ : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private ObjAIBase owner;
        private Particle weaponParticle;
        private bool qConsumed = false;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            owner = ownerSpell.CastInfo.Owner;

            weaponParticle = AddParticleTarget(
                owner,
                owner,
                "Garen_Base_Q_Cas_Sword.troy",
                owner,
                lifetime: 4.5f,
                bone: "weapon"
            );

            // HARD RESET AUTO STATE
            owner.CancelAutoAttack(true);
            owner.SkipNextAutoAttack();

            // FORCE Q AUTO IMMEDIATELY
            owner.SetAutoAttackSpell("GarenQAttack", false);

            ApiEventManager.OnPreAttack.AddListener(this, owner, OnPreAttack);
        }

        private void OnPreAttack(Spell spell)
        {
            // Block ALL attacks except the Q one
            if (spell.SpellName != "GarenQAttack")
            {
                owner.CancelAutoAttack(true);
                owner.SkipNextAutoAttack();
                return;
            }

            // Q attack fired → consume buff
            if (!qConsumed)
            {
                qConsumed = true;
                owner.RemoveBuffsWithName("GarenQ");
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            owner.SetAutoAttackSpell(
                Spells.GarenQ.BASIC_AUTO_ATTACK_SPELL,
                false
            );

            SealSpellSlot(
                owner,
                SpellSlotType.SpellSlots,
                0,
                SpellbookType.SPELLBOOK_CHAMPION,
                false
            );

            ownerSpell.SetCooldown(8);

            if (weaponParticle != null)
                RemoveParticle(weaponParticle);

            ApiEventManager.OnPreAttack.RemoveListener(this);
        }
    }
}
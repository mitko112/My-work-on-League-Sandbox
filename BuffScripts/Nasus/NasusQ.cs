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
    internal class NasusQ : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        Particle pbuff;
        Particle pbuff2;
        Buff thisBuff;
        ObjAIBase owner;

        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            thisBuff = buff;
            if (unit is ObjAIBase ai)
            {
                var owner = ownerSpell.CastInfo.Owner as Champion;
                pbuff = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, "Nasus_Base_Q_Buf.troy", unit, buff.Duration, 1, "BUFFBONE_CSTM_WEAPON_1");
                //pbuff2 = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, "Nasus_Base_Q_Wpn_trail.troy", unit, buff.Duration, 1, "BUFFBONE_CSTM_WEAPON_1");
                StatsModifier.Range.FlatBonus = 50.0f;
                unit.AddStatModifier(StatsModifier);
                //SetAnimStates(owner, new Dictionary<string, string> { { "Attack1", "Spell1" } });
                SealSpellSlot(owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true);
                ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
                owner.CancelAutoAttack(true);
            }
        }

        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner as Champion;
            RemoveParticle(pbuff);
            RemoveParticle(pbuff2);
            RemoveBuff(thisBuff);
            CreateTimer(0.5f, () =>
            {
                //SetAnimStates(owner, new Dictionary<string, string> { { "Spell1", "Attack1" } });
            });
            if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
            if (unit is ObjAIBase ai)
            {
                SealSpellSlot(ai, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
            }
        }

        public void OnLaunchAttack(Spell spell)
        {

            if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {
                spell.CastInfo.Owner.RemoveBuff(thisBuff);
                var owner = spell.CastInfo.Owner as Champion;
                spell.CastInfo.Owner.SkipNextAutoAttack();
                SpellCast(spell.CastInfo.Owner, 0, SpellSlotType.ExtraSlots, false, spell.CastInfo.Owner.TargetUnit, Vector2.Zero);
                SealSpellSlot(owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
                thisBuff.DeactivateBuff();
            }
        }
        public void OnUpdate(float diff)
        {
        }
    }
}

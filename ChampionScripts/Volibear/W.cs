
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class VolibearW : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        private ObjAIBase own;
        Spell Spell;
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Spell = spell;
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, HideW, false);
            own = owner;
        }

        private int hit = 0;

        public void TargetExecute(DamageData data)

        {
            var owner = Spell.CastInfo.Owner;
            AddBuff("VolibearW", 4f, 1, Spell, owner, owner, false);

           
        }

        public void HideW(Spell spell)
        {
            CreateTimer((float)0.1, () => { SealSpellSlot(own, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true); });
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(Spell spell)
        {
            var ap = own.Stats.HealthPoints.FlatBonus * 0.15f;
            LogDebug("HP: " + ap.ToString());
            float damage = (float)(ap + 35 + (own.Spells[1].CastInfo.SpellLevel * 45));
            spell.CastInfo.Targets[0].Unit.TakeDamage(own, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_PROC, false);
            SealSpellSlot(own, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true);
            hit = 0;
            //SealSpellSlot(spell.CastInfo.Owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
        }

        public void OnSpellPostCast(Spell spell)
        {
        }

        public void OnSpellChannel(Spell spell)
        {
        }

        public void OnSpellChannelCancel(Spell spell, ChannelingStopSource source)
        {
        }

        public void OnSpellPostChannel(Spell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            
            {

                
            }
        }
    }
}
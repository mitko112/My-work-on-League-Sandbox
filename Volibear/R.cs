
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
    public class VolibearR : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        private ObjAIBase own;
        Spell Spell;

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, HideE, false);
            ApiEventManager.OnHitUnit.AddListener(this, owner, TargetExecute, false);
            own = owner;
            Spell = spell;
        }

        public void HideE(Spell spell)
        {
        }

        private bool on = false;
        
        public void TargetExecute(DamageData data)

        {
            var unit = Spell.CastInfo.Targets[0].Unit;
            if (on != false)
            {
                LogDebug("LOL");
                var x = GetUnitsInRange(unit.Position, 500, true);
                foreach (var units in x)
                {
                    if (units.Team != own.Team)
                    {
                        float damage = new float[] { 75, 115, 155 }[own.Spells[3].CastInfo.SpellLevel - 1];
                        float ap = own.Stats.AbilityPower.Total * 0.3f;
                        AddParticle(own, units, "volibear_R_chain_lighting_01.troy", own.Position);
                        units.TakeDamage(own, damage + ap, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    }
                }
            }
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(Spell spell)
        {
            //SealSpellSlot(spell.CastInfo.Owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
        }

        public void OnSpellPostCast(Spell spell)
        {
            on = true;
            LogDebug("Yo its NOT over");
            CreateTimer(12.0f, () => { on = false; });
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
        }
    }
}
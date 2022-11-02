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

namespace Spells
{
    public class XenZhaoBattleCry : ISpellScript
    {
        Spell Spell;
        ObjAIBase Owner;
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Spell = spell;
            Owner = spell.CastInfo.Owner;
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, true);

            AddBuff("XenZhaoBattleCryPassive", 1f, 1, spell, owner, owner, true);
        }
        public void OnLevelUp (Spell spell)
        {
            ApiEventManager.OnHitUnit.AddListener(this, spell.CastInfo.Owner, OnHitUnit, false);
        }
        public void OnHitUnit(DamageData damageData)
        {
            AddBuff("XenZhaoBattleCryPassive", 3f, 1, Spell, Owner, Owner);
        }
        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("XenZhaoBattleCry", 5f, 1, Spell, Owner, Owner);
        }

      
        }
    }

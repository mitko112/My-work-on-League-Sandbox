using GameServerCore.Enums;

using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;

namespace Spells
{
    public class VorpalSpikes : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
        }

        public void OnDeactivate(ObjAIBase owner, Spell spell)
        {
        }
        bool toggled = false;
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            toggled = !toggled;
            if (toggled == false)
            {
                RemoveBuff(owner, "VorpalSpikes");
                

                LogDebug("toggled off");
            }
            if (toggled == true)
            {
                AddBuff("VorpalSpikes", 1, 1, spell, target, owner, true);
                
                LogDebug("toggled on");
            }
        }

    }

    public class VorpalSpikesMissle : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
        };


        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        private void TargetExecute(Spell spell, AttackableUnit target, SpellMissile arg3, SpellSector arg4)
        {
            var owner = spell.CastInfo.Owner;
            var ap = owner.Stats.AbilityPower.Total * 0.3f;
            var damage = 5 + spell.CastInfo.SpellLevel * 15 + ap;
            LogDebug("yo");
            target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }

        public void OnUpdate(float diff)
        {
        }
    }

}

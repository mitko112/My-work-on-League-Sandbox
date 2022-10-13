using System.Numerics;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;

namespace Spells
{
    public class Obduracy : ISpellScript
    {
        ObjAIBase Owner;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Owner = owner;

            if (owner is ObjAIBase obj)
            {

                ApiEventManager.OnLaunchAttack.AddListener(this, obj, TargetExecute, true);
            }

        }
        public void TargetExecute(Spell spell)


        {

            var owner = spell.CastInfo.Owner;

            var AD = Owner.Stats.AttackDamage.Total * 0.3f * owner.GetSpell("Obduracy").CastInfo.SpellLevel;
            float damage = AD;





            var units = GetUnitsInRange(owner.Position, 200f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == owner.Team || units[i] is BaseTurret || units[i] is ObjBuilding || units[i] is Inhibitor))
                {
                    units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                    AddParticleTarget(owner, owner, "MalphiteCleaveHit.troy", owner, 1f);

                }
            }
        }



        public void OnSpellCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;

            AddBuff("MalphiteObduracyEffect", 5f, 1, spell, Owner, Owner, false);
            AddParticleTarget(owner, owner, "Malphite_Enrage_glow.troy", owner, 5f);
        }

       
        }
    }

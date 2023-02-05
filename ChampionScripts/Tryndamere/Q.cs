using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;

namespace Spells
{
    public class Bloodlust : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

   
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
            
        }

     

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var APratio = owner.Stats.AbilityPower.Total * 0.3f;



            if (owner.HasBuff("BattleFury"))
            {
                var APRatiofury = owner.Stats.AbilityPower.Total * 0.012f;
                var fury = owner.GetBuffWithName("BattleFury").StackCount;
                var total = fury * 0.5f * APRatiofury;
                float Heali = 30 * spell.CastInfo.SpellLevel + APratio + total;
                owner.Stats.CurrentHealth += Heali;

            }
            else
            {
                float Heal = 30 * spell.CastInfo.SpellLevel + APratio;
                owner.Stats.CurrentHealth += Heal;
            }
            

            
               

             
            AddParticleTarget(owner, owner, "Global_Heal.troy", owner, 1f);

            if (owner.HasBuff("BattleFury"))
                {

                RemoveBuff(owner,"BattleFury");

                }
            

            
        }
        
    }
}
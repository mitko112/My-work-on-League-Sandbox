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
    public class DetonatingShot : ISpellScript
    {
        AttackableUnit Target;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        
            // TODO
        };

        public void OnActivate( Spell spell)
        {
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, true);
        }
        public void OnLevelUp(Spell spell) 
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("DetonatingShotPassive", 1f, 1, spell, owner, owner, true);
        }
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
        }

        public void OnSpellPostCast(Spell spell)
        {

            var owner = spell.CastInfo.Owner;
            AddBuff("DetonatingShot", 5f, 1, spell, Target, owner, false);
              AddParticleTarget(owner, Target, "DetonatingShot_buf.troy", Target, 5f, 1f);
            
        }

       

        public void OnUpdate(float diff)
        {
        }
    }
}
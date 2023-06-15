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
    public class JudicatorDivineBlessing : ISpellScript
    {
        ObjAIBase Owner;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

           
        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Owner = owner;
        }

        
        public void OnSpellCast(Spell spell)
        {



            var owner = spell.CastInfo.Owner;
            var target = spell.CastInfo.Targets[0].Unit;
            var APratio = Owner.Stats.AbilityPower.Total * 0.35f;
            float Heal = 60 * spell.CastInfo.SpellLevel + APratio;
            target.Stats.CurrentHealth += Heal;
            AddParticleTarget(Owner, target, "Global_Heal.troy", target, 1f);
            AddBuff("JudicatorDivineBlessing", 3f, 1, spell, target, owner, false);

        }

        
    }
}

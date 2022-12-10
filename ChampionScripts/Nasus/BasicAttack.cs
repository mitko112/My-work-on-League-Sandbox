using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class NasusBasicAttack : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };


        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
        }

        public void OnLaunchAttack(Spell spell)
        {
            spell.CastInfo.Owner.SetAutoAttackSpell("NasusBasicAttack2", false);
        }


        public class NasusBasicAttack2 : ISpellScript
        {
            public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
            {
                // TODO
            };



            public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
            {
                ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            }

            public void OnLaunchAttack(Spell spell)
            {
                spell.CastInfo.Owner.SetAutoAttackSpell("NasusBasicAttack", false);
            }
        }
    }
}

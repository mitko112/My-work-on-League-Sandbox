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

namespace Spells
{
    public class RyzeBasicAttack : ISpellScript
    {
        ObjAIBase _owner;
        AttackableUnit _targ;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Arc
            }
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            _owner = owner;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
        }

      
        public void OnSpellPreCast(ObjAIBase owner, Spell spell, AttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        // SCUFFED AS SHIT
        // BUT IT FEELS BETTER THAN
        // INSTANT DAMAGE

        public void OnLaunchAttack(Spell spell)
        {
            _targ = _owner.TargetUnit;
            LogDebug(" yo 1");
            var x = _owner.GetSpell("RyzeBasicAttack").CreateSpellMissile(ScriptMetadata.MissileParameters);
            x.SetSpeed(2400f);

            // in ryzebasicattack.json
            //            "MissileSpeed": "2400.0000",

            ApiEventManager.OnSpellMissileEnd.AddListener(this, x, OnSpellHit, true);
        }

        public void OnSpellHit(SpellMissile mis)
        {
            LogDebug("yo 2");
            _targ.TakeDamage(_owner, _owner.Stats.AttackDamage.Total, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }

        
        public void OnUpdate(float diff)
        {
        }
    }
}
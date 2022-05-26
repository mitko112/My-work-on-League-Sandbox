
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class RyzeBasicAttack : ISpellScript
    {
        IObjAiBase _owner;
        IAttackableUnit _targ;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Arc
            }
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        // SCUFFED AS SHIT
        // BUT IT FEELS BETTER THAN
        // INSTANT DAMAGE

        public void OnLaunchAttack(ISpell spell)
        {
            _targ = _owner.TargetUnit;
            LogDebug(" yo 1");
            var x = _owner.GetSpell("RyzeBasicAttack").CreateSpellMissile(ScriptMetadata.MissileParameters);
            x.SetSpeed(2400f);

            // in ryzebasicattack.json
            //            "MissileSpeed": "2400.0000",

            ApiEventManager.OnSpellMissileEnd.AddListener(this, x, OnSpellHit, true);
        }

        public void OnSpellHit(ISpellMissile mis)
        {
            LogDebug("yo 2");
            _targ.TakeDamage(_owner, _owner.Stats.AttackDamage.Total, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
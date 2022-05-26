using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class SpellFlux : ISpellScript
    {

        IObjAiBase _owner;
        IAttackableUnit _targ;

        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Arc
            },
            CastTime = 0.25f
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            ApiEventManager.OnSpellCast.AddListener(this, spell, TargetExecute);
        }
        float ap;
        float dmg;
        public void TargetExecute(ISpell spell)
        {

            ap = spell.CastInfo.Owner.Stats.AbilityPower.Total;
            dmg = 30 + (float)(ap / 3) + spell.CastInfo.SpellLevel * 20;

            _targ = spell.CastInfo.Targets[0].Unit;
            LogDebug(" yo 1");
            var x = _owner.GetSpell("SpellFlux").CreateSpellMissile(ScriptMetadata.MissileParameters);
            x.SetSpeed(1000f);
            // in spellfluxmissile.json
            // "MissileSpeed": "1000.0000",
            ApiEventManager.OnSpellMissileEnd.AddListener(this, x, OnSpellEnd, true);
        }
        public void OnSpellEnd(ISpellMissile mis)
        {

            _owner.CancelAutoAttack(false);

            _targ.TakeDamage(_owner, dmg, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

            if (_owner.HasBuff("DesperatePower"))
            {
                AddParticle(_owner, _targ, "DesperatePower_aoe.troy", _targ.Position);
                var u = GetUnitsInRange(_targ.Position, 300, true);
                foreach (var unit in u)
                {
                    if (unit.Team != _owner.Team)
                    {
                        unit.TakeDamage(_owner, dmg / 2, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    }
                }
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            owner.Spells[0].LowerCooldown((float)1.5);   //Q
            owner.Spells[1].LowerCooldown((float)1.5);   //W
            //owner.Spells[2].LowerCooldown((float)1.5);   //E
            owner.Spells[3].LowerCooldown((float)1.5);   //R
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
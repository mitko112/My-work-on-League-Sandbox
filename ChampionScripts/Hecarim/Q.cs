using System.Linq;
using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class HecarimRapidSlash : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner as IChampion;
            var ad = spell.CastInfo.Owner.Stats.AttackDamage.FlatBonus * 0.6f;
            var admm = spell.CastInfo.Owner.Stats.AttackDamage.FlatBonus * 0.4f;
            var damage = 60*spell.CastInfo.SpellLevel  + ad;
            var damagemm = 40*spell.CastInfo.SpellLevel  + admm;

            AddParticle(owner, null, "Hecarim_Q_tar.troy", owner.Position, direction: owner.Direction);
            AddParticle(owner, null, "Hecarim_Q.troy", owner.Position, direction: owner.Direction);
            AddParticle(owner, null, "Hecarim_Q_weapon.troy", owner.Position, direction: owner.Direction);
            PlayAnimation(owner, "Spell1_Upperbody", 0.7f);
            var units = GetUnitsInRange(owner.Position, 425f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == owner.Team || units[i] is IBaseTurret || units[i] is IObjBuilding || units[i] is IInhibitor))
                {
                    units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                    


                    if (units[i] is IMinion)

                        units[i].TakeDamage(owner, damagemm, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    {
                        if (units[i] is IMonster)
                            units[i].TakeDamage(owner, damagemm, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    }

                }
            }

        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell)
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
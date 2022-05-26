using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
namespace Buffs


{
    internal class VorpalSpikes : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };
        

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        


        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {

            if (unit is IObjAiBase obj)
            {
                ApiEventManager.OnLaunchAttack.AddListener(this, obj, TargetExecute, false);




            }
        }
        public void TargetExecute(ISpell spell)


        {







            var owner = spell.CastInfo.Owner;
            var target = spell.CastInfo.Targets[0].Unit;
            float AP = owner.Stats.AbilityPower.Total * 0.3f;
            float damage = 20 * owner.GetSpell(2).CastInfo.SpellLevel + AP;



            
            //AddParticleTarget(owner, owner, "particlename.troy", owner, 1f);


            var units = GetUnitsInRange(owner.Position, 500f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == owner.Team || units[i] is IBaseTurret || units[i] is IObjBuilding || units[i] is IInhibitor))
                {
                    units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);

                }
            }
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);

        }
        
        public void OnUpdate(float diff)
        {

        }
    }
}
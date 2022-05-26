using System.Numerics;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace Buffs
{
    internal class PowerFist: IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };
        
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IBuff thisBuff;
        IObjAiBase Unit;
        
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisBuff = buff;
            if (unit is IObjAiBase ai)
            {
                Unit = ai;

                ApiEventManager.OnHitUnit.AddListener(this, ai, TargetExecute, true);

                ai.SkipNextAutoAttack();


            }
            
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //ApiEventManager.OnHitUnit.RemoveListener(this);

        }
        public void TargetExecute(IDamageData damageData)
        {
            if (!thisBuff.Elapsed() && thisBuff != null && Unit != null)
            {
                float ad = Unit.Stats.AttackDamage.Total * 1.5f;
                var target = damageData.Target;
                float damage = 10*Unit.GetSpell(1).CastInfo.SpellLevel + ad;
                
                
                target.TakeDamage(Unit, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                ForceMovement(target, "RUN", new Vector2(target.Position.X + 5f, target.Position.Y + 5f), 13f, 0, 16.5f, 0);
                thisBuff.DeactivateBuff();
            }
        }
        public void OnUpdate(float diff)
        {

        }
    }
}
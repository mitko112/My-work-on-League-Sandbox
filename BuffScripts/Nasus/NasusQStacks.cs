using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class NasusQStacks : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COUNTER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 999999
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IBuff thisBuff;
        IObjAiBase Unit;
        IParticle p;
        IParticle p2;
        IObjAiBase Owner;
        IAttackableUnit AtOwner;
        public static float Qstacks = 0f;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //ApiEventManager.OnKillUnit.AddListener(this, AtOwner, NasusMoreStacks, true);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //ApiEventManager.OnHitUnit.RemoveListener(this);

        }

        public void OnUpdate(float diff)
        {

        }
    }
}
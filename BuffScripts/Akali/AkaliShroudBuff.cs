using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class AkaliShroudBuff : IBuffGameScript
    {
        public BuffType BuffType => BuffType.COMBAT_DEHANCER;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            var Champs = GetChampionsInRange(unit.Position, 50000, true);
            foreach (IChampion player in Champs)
            {
                if (player.Team.Equals(owner.Team))
                {
                    owner.SetInvisible((int)player.GetPlayerId(), owner, 0.5f, 0.1f);
                }
                if (!(player.Team.Equals(owner.Team)))
                {
                    if (player.IsAttacking)
                    {
                        player.CancelAutoAttack(false);
                    }
                    owner.SetInvisible((int)player.GetPlayerId(), owner, 0f, 0.1f);
                }
            }
            unit.SetStatus(StatusFlags.Targetable, false);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            var Champs = GetChampionsInRange(unit.Position, 50000, true);
            foreach (IChampion player in Champs)
            {
                owner.SetInvisible((int)player.GetPlayerId(), owner, 1f, 0.1f);
            }
            unit.SetStatus(StatusFlags.Targetable, true);
        }

        public void OnPreAttack(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
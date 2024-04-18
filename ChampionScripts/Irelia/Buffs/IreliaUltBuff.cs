using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects;


namespace Buffs
{
    internal class IreliaTranscendentBlades : IBuffGameScript
    {
        Particle R1;
        Particle R2;
        Particle L1;
        Particle L2;
        Particle M1;
        ObjAIBase Irelia;
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 4
        };
        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            Irelia = ownerSpell.CastInfo.Owner as Champion;
            Irelia.Spells[3].SetCooldown(0.5f, true);
            var trueCoords = new Vector2(ownerSpell.CastInfo.TargetPosition.X, ownerSpell.CastInfo.TargetPosition.Z);
            SpellCast(Irelia, 0, SpellSlotType.ExtraSlots, trueCoords, trueCoords, true, Vector2.Zero);
            switch (buff.StackCount)
            {
                case 1:
                    PlayAnimation(Irelia, "Spell4");
                    R1 = AddParticleTarget(Irelia, Irelia, "irelia_ult_dagger_active_04.troy", Irelia, 25000, 1, "BUFFBONE_CSTM_DAGGER5");
                    L1 = AddParticleTarget(Irelia, Irelia, "irelia_ult_dagger_active_04.troy", Irelia, 25000, 1, "BUFFBONE_CSTM_DAGGER2");
                    R2 = AddParticleTarget(Irelia, Irelia, "irelia_ult_dagger_active_04.troy", Irelia, 25000, 1, "BUFFBONE_CSTM_DAGGER4");
                    L2 = AddParticleTarget(Irelia, Irelia, "irelia_ult_dagger_active_04.troy", Irelia, 25000, 1, "BUFFBONE_CSTM_DAGGER1");
                    //p5 = AddParticleTarget(Irelia, Irelia, "irelia_ult_dagger_active_04.troy", Irelia, buff.Duration,1,"BUFFBONE_Cstm_Sword1_loc");
                    M1 = AddParticleTarget(Irelia, Irelia, "irelia_ult_magic_resist.troy", Irelia, 25000);
                    RemoveParticle(R1);
                    break;
                case 2:
                    RemoveParticle(L1);
                    break;
                case 3:
                    RemoveParticle(R2);
                    break;
                case 4:
                    RemoveParticle(L2);
                    buff.DeactivateBuff();
                    break;
            }
        }
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(R1);
            RemoveParticle(L1);
            RemoveParticle(R2);
            RemoveParticle(L2);
            RemoveParticle(M1);
            Irelia.Spells[3].SetCooldown((80f - (10 * Irelia.Spells[3].CastInfo.SpellLevel)) * (1 + Irelia.Stats.CooldownReduction.Total), true);
        }
    }
}
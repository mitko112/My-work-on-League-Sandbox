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
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;

using System;

namespace Spells
{
    public class Bloodlust : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        private readonly float[] HEALS_PER_FURY = { 0.5f, 0.95f, 1.4f, 1.85f, 2.3f };

        public void OnSpellPostCast(Spell spell)
        {
            ObjAIBase owner = spell.CastInfo.Owner;

            float baseHeal = 30f + 10f * (spell.CastInfo.SpellLevel - 1);
            float healBonus = owner.Stats.AbilityPower.Total * 0.3f;
            float flatHeal = baseHeal + healBonus;

            float perFuryPointHealBonus = owner.Stats.AbilityPower.Total * 0.012f;
            float fury = owner.Stats.CurrentMana;
            float healPerFury = HEALS_PER_FURY[spell.CastInfo.SpellLevel - 1] + perFuryPointHealBonus;

            float totalHeal = flatHeal + fury * healPerFury;

            owner.TakeHeal(owner, totalHeal);
            owner.Stats.CurrentMana = 0;

            if (owner.HasBuff("BattleFury"))
            {
                RemoveBuff(owner, "BattleFury");
            }

            AddParticleTarget(owner, owner, "Global_Heal.troy", owner, 1f);
        }
    }
}
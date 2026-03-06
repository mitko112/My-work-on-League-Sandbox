using GameMaths;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using Microsoft.CodeAnalysis.Operations;
using Spells;
using System;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics;
using System.Text.RegularExpressions;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace Buffs
{
    internal class MockingShoutSlow : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.SLOW,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private bool IsFacingAway(AttackableUnit target, AttackableUnit caster)
        {
            // Target rotation (radians)
            float rot = target.Position.Y;

            // Target forward vector
            float forwardX = (float)Math.Cos(rot);
            float forwardY = (float)Math.Sin(rot);

            // Direction from target to caster
            var dirX = caster.Position.X - target.Position.X;
            var dirY = caster.Position.Y - target.Position.Y;

            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            if (length <= 0.001f)
                return false;

            dirX /= length;
            dirY /= length;

            // Dot product
            float dot = (forwardX * dirX) + (forwardY * dirY);

            // Facing away
            return dot < 0f;
        }




        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            var caster = ownerSpell.CastInfo.Owner;

            // ✅ ALWAYS reduce attack damage
            StatsModifier.AttackDamage.FlatBonus -= 20f;

            // ✅ Only slow if target is facing away
            if (IsFacingAway(unit, caster))
            {
                StatsModifier.MoveSpeed.PercentBonus -= 0.4f;

                AddParticleTarget(
                    caster,
                    unit,
                    "Global_Slow.troy",
                    unit,
                    buff.Duration
                );
            }

            unit.AddStatModifier(StatsModifier);
        }
        
            
        





        public void OnUpdate(float diff)
        {

        }
    }
}
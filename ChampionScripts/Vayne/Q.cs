
using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;

namespace Spells
{
    public class VayneTumble : ISpellScript
    {
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = false,
            NotSingleTargetSpell = true
            // TODO
        };

      

        public void OnSpellPostCast(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
            var trueCoords = GetPointFromUnit(owner, 300f);

            ForceMovement(owner, "Spell3", trueCoords, 1200, 0, 0, 0);
            AddBuff("VayneTumble", 4.0f, 1, spell, owner, owner);
        }

        }
    }

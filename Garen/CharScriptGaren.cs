using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using            GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Logging;
using System.Threading.Tasks;
using System;
using System.Threading;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using System.Linq;

namespace CharScripts
{
    public class CharScriptGaren : ICharScript
    {
        private static readonly float[] OUT_OF_COMBAT_COOLDOWNS = { 9, 6, 4 };

        private static readonly UnitTag[] MINION_UNIT_TAG_PASSIVE_EXCEPTIONS =
        {
            UnitTag.Minion,
            UnitTag.Minion_Lane,
            UnitTag.Minion_Lane_Siege,
            UnitTag.Minion_Lane_Super,
            UnitTag.Minion_Summon,
        };

        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            spell.LoadScript();

            RunOnGameStart(() =>
            {
                AddBuff("GarenPassive", 99999f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner, true);
            });
        }

        public static void RunOnGameStart(Action methodToRunAfterStart)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (ApiMapFunctionManager.GameTime() > 1f)
                    {
                        methodToRunAfterStart.Invoke();
                        break;
                    }
                    Thread.Sleep(1000);
                }
            });
        }

        public static int GetCorrectLevelIndex(int level)
        {
            if (level >= 16)
            {
                return 2;
            }
            else if (level >= 11)
            {
                return 1;
            }
            return 0;
        }

        public static float GetCooldownForLevel(int level)
        {
            return OUT_OF_COMBAT_COOLDOWNS[GetCorrectLevelIndex(level)];
        }

        public static bool ShouldPassiveTurnOff(AttackableUnit unit, DamageData damageData)
        {
            if (MINION_UNIT_TAG_PASSIVE_EXCEPTIONS.Contains(damageData.Attacker.CharData.UnitTags))
            {
                return false;
            }

            if (unit.Stats.Level >= 11 && UnitTag.Monster.Equals(damageData.Attacker.CharData.UnitTags))
            {
                return false;
            }

            if (damageData.PostMitigationDamage <= 0)
            {
                return false;
            }

            return true;
        }
    }
}


using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.API;
using GameServerLib.GameObjects.AttackableUnits;

namespace CharScripts
{
    public class CharScriptTristana : ICharScript
    {
        ObjAIBase Owner;
        public void OnActivate(ObjAIBase owner, Spell Spell)
        {
            Owner = owner;
            ApiEventManager.OnKill.AddListener(this, owner, OnKill, false);
            ApiEventManager.OnLevelUp.AddListener(this, owner, OnLevelUp, false);
        }
        public void OnKill(DeathData Data)
        {
            Owner.GetSpell("RocketJump").SetCooldown(0f, true);
        }
        public void OnLevelUp(AttackableUnit Unit)
        {
            Owner.Stats.Range.FlatBonus = 8 * ((Owner as Champion).Stats.Level - 1); 
        }
    }
}

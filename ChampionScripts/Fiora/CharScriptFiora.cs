using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using System.Collections.Generic;
using LeagueSandbox.GameServer.GameObjects;
namespace CharScripts
{
    public class CharScriptFiora : ICharScript
    {
        ObjAIBase Fiora;
        AttackableUnit Target;
        public void OnActivate(ObjAIBase owner, Spell spell = null)
        {
            Fiora = owner as Champion;
            ApiEventManager.OnLaunchAttack.AddListener(this, Fiora, OnLaunchAttack, false);
        }
        public void OnLaunchAttack(Spell spell)
        {
            Target = Fiora.TargetUnit;
            if (Target is Champion)
            {
                if (Fiora.HasBuff("FioraDuelistHotParticle2"))
                {
                    Fiora.RemoveBuffsWithName("FioraDuelistHotParticle2");
                    AddBuff("FioraDuelistHotParticle", 6f, 2, spell, Fiora, Fiora);
                }
                else
                {
                    AddBuff("FioraDuelistHotParticle", 6f, 1, spell, Fiora, Fiora);
                }
            }
            else
            {
                if (!Fiora.HasBuff("FioraDuelistHotParticle"))
                {
                    AddBuff("FioraDuelistHotParticle2", 6f, 1, spell, Fiora, Fiora);
                }
            }
        }
    }
}

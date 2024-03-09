using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.Buildings.AnimatedBuildings;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;



namespace CharScripts
{

    public class CharScriptGragas : ICharScript

    {
        
       

            public void OnActivate(ObjAIBase owner, Spell spell)
        {
            ApiEventManager.OnSpellCast.AddListener(owner, owner.GetSpell("GragasQ"), ApplyHeal, false);
            ApiEventManager.OnSpellCast.AddListener(owner, owner.GetSpell("GragasW"), ApplyHeal, false);
            ApiEventManager.OnSpellCast.AddListener(owner, owner.GetSpell("GragasWAttack"), ApplyHeal, false);
            ApiEventManager.OnSpellChannel.AddListener(owner, owner.GetSpell("GragasW"), ApplyHeal, false);
            ApiEventManager.OnSpellCast.AddListener(owner, owner.GetSpell("GragasE"), ApplyHeal, false);
            ApiEventManager.OnSpellCast.AddListener(owner, owner.GetSpell("GragasR"), ApplyHeal, false);
        }

        public void ApplyHeal(Spell spell)
        {
            var owner = spell.CastInfo.Owner;
                var Heal = owner.Stats.HealthPoints.Total * 0.04f;
                    owner.Stats.CurrentHealth += Heal;
                AddParticleTarget(owner, owner, "Global_Heal", owner, 1, 1f);
                
            }
        }
    }




 
       
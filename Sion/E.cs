
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.SpellNS;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Missile;
using LeagueSandbox.GameServer.GameObjects.SpellNS.Sector;
using LeagueSandbox.GameServer.API;
using GameServerLib.GameObjects.AttackableUnits;

namespace Spells
{
    public class SionE : ISpellScript
    {
        ObjAIBase Owner;
        float timeSinceLastTick = 1000f;
        Spell Spell;
        public SpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(ObjAIBase owner, Spell spell)
        {
            Owner = owner;
            Spell = spell;
            ApiEventManager.OnKillUnit.AddListener(this, owner, OnKillUnit, false);
        }
        public void OnKillUnit(DeathData data)
        {
            AddBuff("SionEnrageHP", 1, 1, Spell, Owner, Owner, true);
        }
       
        public void OnSpellPostCast(Spell spell)
        {
            if (!Owner.HasBuff("SionEnrage"))
            {
                AddBuff("SionEnrage", 1, 1, spell, Owner, Owner, true);
                timeSinceLastTick = 1000f;
                spell.SetCooldown(0.5f, true);
            }
            else
            {
                RemoveBuff(Owner, "SionEnrage");
            }
        }

        
            }
        }
    

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
using LeagueSandbox.GameServer.GameObjects.StatsNS;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;



namespace Buffs
{
    internal class SionWShield : IBuffGameScript
    {
        public BuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.SPELL_SHIELD,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public StatsModifier StatsModifier { get; private set; } = new StatsModifier();


        Spell Spell;
        Buff Buff;
        Particle p;
       
        public void OnActivate(AttackableUnit unit, Buff buff, Spell ownerSpell)


        {
            Buff = buff;
            var owner = ownerSpell.CastInfo.Owner;
            Spell = ownerSpell;
            p = AddParticleTarget(owner, owner, "deathscaress_buf.troy", owner, buff.Duration);

            ApiEventManager.OnTakeDamage.AddListener(this, owner, OnTakeDamage, false);
        }
        public void OnTakeDamage(DamageData data)
        {
            var owner = Spell.CastInfo.Owner;
            //var reduction = //owner.GetSpell("SionW").CastInfo.SpellLevel;
            var AP = owner.Stats.AbilityPower.Total * 0.9f;

            data.Damage = -100 * AP; //* //reduction;

            

                RemoveBuff(Buff);
        }
    
        public void OnDeactivate(AttackableUnit unit, Buff buff, Spell ownerSpell)
        {
            RemoveParticle(p);


            var owner = ownerSpell.CastInfo.Owner as Champion;
            var ap = ownerSpell.CastInfo.Owner.Stats.AbilityPower.Total * 0.9f;
            var damage = 70 * ownerSpell.CastInfo.SpellLevel + ap;

            AddParticleTarget(owner, owner, "DeathsCaress_nova.troy", owner, 1f);


            var units = GetUnitsInRange(owner.Position, 550f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == owner.Team || units[i] is BaseTurret || units[i] is ObjBuilding || units[i] is Inhibitor))
                {
                    units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);

                    RemoveBuff(owner, "SionWSwitch");
                }
            }




            }

        public void OnUpdate(float diff)
        {
            
        }
    }
}

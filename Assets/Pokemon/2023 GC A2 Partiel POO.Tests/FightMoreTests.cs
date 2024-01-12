
using System;
using _2023_GC_A2_Partiel_POO.Level_2;
using NUnit.Framework;
using UnityEngine;

namespace _2023_GC_A2_Partiel_POO.Tests.Level_2
{
    public class FightMoreTests
    {
        // Tu as probablement remarqué qu'il y a encore beaucoup de code qui n'a pas été testé ...
        // À présent c'est à toi de créer les TU sur le reste et de les implémenter
        
        // Ce que tu peux ajouter:
        // - Ajouter davantage de sécurité sur les tests apportés
            // - un heal ne régénère pas plus que les HP Max
            // - si on abaisse les HPMax les HP courant doivent suivre si c'est au dessus de la nouvelle valeur
            // - ajouter un equipement qui rend les attaques prioritaires puis l'enlever et voir que l'attaque n'est plus prioritaire etc)
        // - Le support des status (sleep et burn) qui font des effets à la fin du tour et/ou empeche le pkmn d'agir
        // - Gérer la notion de force/faiblesse avec les différentes attaques à disposition (skills.cs)
        // - Cumuler les force/faiblesses en ajoutant un type pour l'équipement qui rendrait plus sensible/résistant à un type

        [Test]
        public void TypeResolverTest()
        {
            

            Assert.That(TypeResolver.GetFactor(TYPE.WATER, TYPE.FIRE), Is.EqualTo(1.2f));
            Assert.That(TypeResolver.GetFactor(TYPE.FIRE, TYPE.FIRE), Is.EqualTo(0.8f));
            Assert.That(TypeResolver.GetFactor(TYPE.GRASS, TYPE.WATER), Is.EqualTo(1.2f));
            Assert.That(TypeResolver.GetFactor(TYPE.WATER, TYPE.FIRE), Is.EqualTo(1.2f));
            Assert.That(TypeResolver.GetFactor(TYPE.GRASS, TYPE.FIRE), Is.EqualTo(0.8f));

        }

        [Test]
        public void ReceivePunchWithElements()
        {
            for (int i = 0; i < 4; i++)
            {
                TYPE type = (TYPE)i;

                Character pok = new(100, 50, 30, 20, type);

                for (int j = 0; j < 4; j++)
                {
                    float oldHealth = pok.CurrentHealth;
                    TYPE atkType = (TYPE)j;

                    Skill atk = new(atkType, 20, StatusPotential.NONE);
                    
                    pok.ReceiveAttack(atk);
                    
                    Assert.That(pok.CurrentHealth, 
                        Is.EqualTo(Math.Clamp(oldHealth - (atk.Power * TypeResolver.GetFactor(atk.Type, pok.BaseType) - pok.Defense), 0, pok.MaxHealth))); 

                    pok.Heal(256);

                }


            }
           
        }
        
        [Test]
        public void FireBallEffectTest()
        {
            Character charizard = new(100, 10, 30, 2000, TYPE.FIRE);
            Character theRock = new(10000, 10, 0, 2, TYPE.ROCK);
            float oldRockHealth = theRock.CurrentHealth;
            
            Fight f = new(charizard, theRock);
            FireBall fireBall = new();
            Skill nullSkill = new(0, 0, 0);

            f.ExecuteTurn(fireBall, nullSkill);

            // Comme The Rock vient apres Charizard il se prend des dmg de brulûre d'ou le -10 bonus
            Assert.That(theRock.CurrentHealth, Is.EqualTo(Math.Clamp(oldRockHealth - 10 - (fireBall.Power * TypeResolver.GetFactor(fireBall.Type, theRock.BaseType) - theRock.Defense), 0, theRock.MaxHealth)));
            oldRockHealth = theRock.CurrentHealth;

            // Let burn stop
            for (int i = 0; i < 4; i++)
            {
                f.ExecuteTurn(nullSkill, nullSkill);
                Assert.That(theRock.CurrentHealth, Is.EqualTo(Math.Clamp(oldRockHealth - 10, 0, theRock.MaxHealth)));
                oldRockHealth = theRock.CurrentHealth;
            }
            
            f.ExecuteTurn(nullSkill, nullSkill);
            Assert.That(theRock.CurrentHealth, Is.EqualTo(Math.Clamp(oldRockHealth, 0, theRock.MaxHealth)));
        }
        
        [Test]
        public void DrugThatHurtsEffectTest()
        {
            Character junkie = new(100, 10, 30, 2000, TYPE.FIRE);
            Character theRock = new(10000, 10, 0, 2, TYPE.ROCK);
            float oldRockHealth = theRock.CurrentHealth;
            
            Fight f = new(junkie, theRock);
            Skill needleThrow = new(TYPE.NORMAL, 10, StatusPotential.POISON);
            Skill nullSkill = new(0, 0, 0);

            f.ExecuteTurn(needleThrow, nullSkill);

            // Comme The Rock vient apres junkie il se prend des dmg de poison d'ou le -15 bonus
            Assert.That(theRock.CurrentHealth, Is.EqualTo(Math.Clamp(oldRockHealth - 15 - (needleThrow.Power * TypeResolver.GetFactor(needleThrow.Type, theRock.BaseType) - theRock.Defense), 0, theRock.MaxHealth)));
            oldRockHealth = theRock.CurrentHealth;
            
            // Let poison stop
            for (int i = 0; i < 2; i++)
            {
                f.ExecuteTurn(nullSkill, nullSkill);
                Assert.That(theRock.CurrentHealth, Is.EqualTo(Math.Clamp(oldRockHealth - 15, 0, theRock.MaxHealth)));
                oldRockHealth = theRock.CurrentHealth;
            }
            
            f.ExecuteTurn(nullSkill, nullSkill);
            Assert.That(theRock.CurrentHealth, Is.EqualTo(Math.Clamp(oldRockHealth, 0, theRock.MaxHealth)));
        }
        
        [Test]
        public void FaisDodoEffectTest()
        {
            Character sandman = new(100, 10, 0, 2000, TYPE.FIRE);
            Character theRock = new(10000, 10, 0, 2, TYPE.ROCK);
            float oldSandmanHealth = sandman.CurrentHealth;
            
            Fight f = new(sandman, theRock);
            MagicalGrass magicalGrass = new();
            Skill nullSkill = new(0, 0, 0);
            Skill smallPunch = new(TYPE.FIRE, 10, 0);

            f.ExecuteTurn(magicalGrass, smallPunch);

            Assert.That(sandman.CurrentHealth, Is.EqualTo(oldSandmanHealth));

            for (int i = 0; i < 3; i++)
            {
                f.ExecuteTurn(nullSkill, smallPunch);
                Assert.That(sandman.CurrentHealth, Is.EqualTo(oldSandmanHealth));
            }
            
            // Last turn of sleep
            f.ExecuteTurn(nullSkill, smallPunch);
            Assert.That(sandman.CurrentHealth, Is.EqualTo(oldSandmanHealth));
            
            f.ExecuteTurn(nullSkill, smallPunch);
            Assert.That(sandman.CurrentHealth, Is.EqualTo(oldSandmanHealth - (smallPunch.Power * TypeResolver.GetFactor(smallPunch.Type, sandman.BaseType) - sandman.Defense)));
        }
        
        [Test]
        public void CrazyEffectTest()
        {
            Character madMan = new(100, 10, 0, 2000, TYPE.NORMAL);
            Character theRock = new(10000, 10, 0, 2, TYPE.ROCK);
            float oldMadManHealth = madMan.CurrentHealth;
            
            madMan.ApplyNewStatusEffect(StatusPotential.CRAZY);

            Fight f = new(madMan, theRock);
            
            Skill smallPunch = new(0, 100, StatusPotential.NONE);
            Skill nullSkill = new(0, 0, 0);
            
            f.ExecuteTurn(smallPunch, nullSkill);
            
            // 0.3f is the crazy value return dmg
            Assert.That(madMan.CurrentHealth, Is.EqualTo(oldMadManHealth - smallPunch.Power * 0.3f));
            oldMadManHealth = madMan.CurrentHealth;

            f.ExecuteTurn(smallPunch, nullSkill);
            
            // crazy should not be active anymore
            Assert.That(madMan.CurrentHealth, Is.EqualTo(oldMadManHealth));
            
        }

        [Test]
        public void ItemTests()
        {
            Character dummy = new(1000, 0, 0, 0, TYPE.NORMAL);
            
            dummy.TakeDamage(200);
            Potion potion = new(150);
            potion.UseOn(dummy);
            Assert.That(dummy.CurrentHealth, Is.EqualTo(950));
            potion.UseOn(dummy);
            Assert.That(dummy.CurrentHealth, Is.EqualTo(950));
            
            dummy.ApplyNewStatusEffect(StatusPotential.SLEEP);
            WakeUpThisNoHotel wakeUp = new();

            wakeUp.UseOn(dummy);
            Assert.That(dummy.CurrentStatus, Is.EqualTo(null));
            wakeUp = new WakeUpThisNoHotel();
            
            dummy.ApplyNewStatusEffect(StatusPotential.BURN);
            IceBucket iceBucket = new();

            wakeUp.UseOn(dummy);
            Assert.That(dummy.CurrentStatus, Is.TypeOf(typeof(BurnStatus)));
            
            iceBucket.UseOn(dummy);
            
            Assert.That(dummy.CurrentStatus, Is.EqualTo(null));
            
            dummy.ApplyNewStatusEffect(StatusPotential.BURN);
            Assert.That(dummy.CurrentStatus, Is.TypeOf(typeof(BurnStatus)));
            iceBucket.UseOn(dummy);
            Assert.That(dummy.CurrentStatus, Is.TypeOf(typeof(BurnStatus)));
            
            iceBucket = new IceBucket();
            iceBucket.UseOn(dummy);
            Assert.That(dummy.CurrentStatus, Is.EqualTo(null));

            wakeUp = new WakeUpThisNoHotel();
            
            dummy.ApplyNewStatusEffect(StatusPotential.POISON);
            RehabFreeEntry rehab = new();

            wakeUp.UseOn(dummy);
            Assert.That(dummy.CurrentStatus, Is.TypeOf(typeof(PoisonStatus)));
            
            
            rehab.UseOn(dummy);
            Assert.That(dummy.CurrentStatus, Is.EqualTo(null));

            rehab = new RehabFreeEntry();

            rehab.UseOn(dummy);
            dummy.ApplyNewStatusEffect(StatusPotential.POISON);
            rehab.UseOn(dummy);

            Assert.That(dummy.CurrentStatus, Is.EqualTo(null));

        }
        
    }
}

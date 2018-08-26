using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoMInterface.Game.Enums;
using WoMInterface.Game.Random;

namespace WoMInterface.Game.Model
{   
    public class Rat : Monster
    {
        // N Tiny animal
        // - DEFENSE
        // Fort +2, Ref +4, Will +1
        // - OFFENSE
        // Speed 15 ft., climb 15 ft., swim 15 ft.
        // Melee bite +4 (1d3–4)
        // Space 2-1/2 ft.; Reach 0 ft.
        // - STATISTICS
        // CMB +0; CMD 6 (10 vs.trip)
        // Feats Weapon Finesse
        // Skills Climb +10, Stealth +18, Swim +10; Racial Modifiers +4 Stealth
        // - ECOLOGY
        // Environment: any temperate
        // Organization: solitary, pair, nest(3–12), or plague(13–100)
        public Rat() : base("Rat", 0.25, SizeType.TINY, MonsterType.ANIMALS, 100, null)
        {
            Strength = 2;
            Dexterity = 15;
            Constitution = 11;
            Inteligence = 2;
            Wisdom = 13;
            Charisma = 2;

            NaturalArmor = 2;

            HitPointDiceRollEvent = new int[] { 1, 8, 0 };
            HitPointDice = 8;

            BaseAttackBonus = 0;

            Equipment.BaseWeapon = new Weapon("Bite", new int[] { 1, 3, 0 });

            Description =
                "Fecund and secretive, rats are omnivorous rodents that particularly thrive in urban areas.";
        }
    }

    public class Wolf : Monster
    {
        // N Medium animal
        // - DEFENSE
        // Fort +5, Ref +5, Will +1
        // - OFFENSE
        // Speed 50 ft.
        // Melee bite +2 (1d6+1 plus trip)
        // - STATISTICS
        // CMB +2; CMD 14 (18 vs.trip)
        // Feats Skill: Focus(Perception)
        // Skills: Perception +8, Stealth +6, Survival +1 (+5 scent tracking); Racial Modifiers +4 Survival when tracking by scent
        // - ECOLOGY
        // Environment: cold or temperate forests
        // Organization: solitary, pair, or pack(3–12)
        public Wolf() : base("Wolf", 1, SizeType.MEDIUM, MonsterType.ANIMALS, 400, null)
        {
            Strength = 13;
            Dexterity = 15;
            Constitution = 15;
            Inteligence = 2;
            Wisdom = 12;
            Charisma = 6;

            NaturalArmor = 2;

            HitPointDiceRollEvent = new int[] { 2, 8, 0, 4 };
            HitPointDice = 20;
            
            BaseAttackBonus = 1;

            Equipment.BaseWeapon = new Weapon("Bite", new int[] { 1, 6, 0});

            Description = 
                "Wandering alone or in packs, wolves sit at the top of the food chain. Ferociously " +
                "territorial and exceptionally wide-ranging in their hunting, wolf packs cover broad " +
                "areas. A wolf’s wide paws contain slight webbing between the toes that assists in " +
                "moving over snow, and its fur is a thick, water-resistant coat ranging in color from " +
                "gray to brown and even black in some species. Its paws contain scent glands that mark " +
                "the ground as it travels, assisting in navigation as well as broadcasting its whereabouts " +
                "to fellow pack members. Generally, a wolf stands from 2-1/2 to 3 feet tall at the shoulder " +
                "and weighs between 45 and 150 pounds, with females being slightly smaller.";
        }
    }
}

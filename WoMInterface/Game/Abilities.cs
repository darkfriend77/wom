using System.Collections.Generic;

namespace WoMInterface.Game
{
    public class Abilities
    {
        public Stat StrengthAttr = StatBuilder.Create("Strength")
            .CreationRollEvent(new int[] {4,6,3})
            .Description("A measure of how physically strong a character is. Strength often controls the" +
            " maximum weight the character can carry, melee attack and/or damage, and sometimes hit points. " +
            "Armor and weapons might also have a Strength requirement.")
            .Build();
        public int Strength => StrengthAttr.GetValue();

        public Stat DexterityAttr = StatBuilder.Create("Dexterity")
            .CreationRollEvent(new int[] { 4, 6, 3 })
            .Description("A measure of how agile a character is. Dexterity controls attack and movement speed" +
            " and accuracy, as well as evading an opponent's attack (see Armor Class).")
            .Build();
        public int Dexterity => DexterityAttr.GetValue();

        public Stat ConstitutionAttr = StatBuilder.Create("Constitution")
            .CreationRollEvent(new int[] { 4, 6, 3 })
            .Description("A measure of how sturdy a character is. Constitution often influences hit points," +
            " resistances for special types of damage(poisons, illness, heat etc.) and fatigue.")
            .Build();
        public int Constitution => ConstitutionAttr.GetValue();

        public Stat InteligenceAttr = StatBuilder.Create("Intelligence")
            .CreationRollEvent(new int[] { 4, 6, 3 })
            .Description("A measure of a character's problem-solving ability. Intelligence often controls a " +
            "character's ability to comprehend foreign languages and their skill in magic.In some cases, " +
            "intelligence controls how many skill points the character gets at 'level up'. In some games, " +
            "it controls the rate at which experience points are earned, or the amount needed to level up. Under" +
            " certain circumstances, this skill can also negate combat actions between players and NPC enemies. " +
            "This is sometimes combined with wisdom and/or willpower.")
            .Build();
        public int Inteligence => InteligenceAttr.GetValue();

        public Stat WisdomAttr = StatBuilder.Create("Wisdom")
            .CreationRollEvent(new int[] { 4, 6, 3 })
            .Description("A measure of a character's common sense and/or spirituality. Wisdom often controls a " +
            "character's ability to cast certain spells, communicate to mystical entities, or discern other " +
            "characters' motives or feelings.")
            .Build();
        public int Wisdom => WisdomAttr.GetValue();

        public Stat CharismaAttr = StatBuilder.Create("Charisma")
            .CreationRollEvent(new int[] { 4, 6, 3 })
            .Description("A measure of a character's social skills, and sometimes their physical appearance. " +
            "Charisma generally influences prices while trading and NPC reactions. Under certain circumstances," +
            " this skill can negate combat actions between players and NPC enemies.")
            .Build();
        public int Charisma => CharismaAttr.GetValue();

        public List<Stat> All => new List<Stat>() { StrengthAttr, DexterityAttr, ConstitutionAttr, InteligenceAttr, WisdomAttr, CharismaAttr};

        public Abilities(Shift shift)
        {
            Dice dice = new Dice(shift);
            All.ForEach(p => p.CreateValue(dice));
        }

    }
}
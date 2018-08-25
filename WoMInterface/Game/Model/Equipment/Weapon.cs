namespace WoMInterface.Game.Model
{
    public class Weapon
    {
        public string Name { get; }
        public int[] DamageRoll { get; }

        public Weapon(string name, int[] damageRoll)
        {
            Name = name;
            DamageRoll = damageRoll;
        }

    }

    public class Fist : Weapon
    {
        public Fist() : base("Fist", new int[] {1, 3, 0})
        {
        }
    }
}
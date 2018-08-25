namespace WoMInterface.Game.Model
{
    public class Weapon
    {
        public int[] DamageRoll;
    }

    public class Fist : Weapon
    {
        public Fist()
        {
            base.DamageRoll = new int[] { 1, 6, 1 };
        }
    }
}
namespace WoMInterface.Game.Model
{
    public class Equipment
    {
        public Weapon BaseWeapon { get; set; }

        private Weapon primaryWeapon;
        public Weapon PrimaryWeapon { get { return primaryWeapon ?? BaseWeapon; } set { primaryWeapon = value; } }

        public Weapon SecondaryWeapon { get; set; }

        public int ArmorBonus => 0;

        public int ShieldBonus => 0;
    }
}
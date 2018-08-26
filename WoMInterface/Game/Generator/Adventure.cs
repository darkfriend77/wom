using WoMInterface.Game.Combat;
using WoMInterface.Game.Interaction;

namespace WoMInterface.Game.Generator
{
    public abstract class Adventure
    {
        public AdventureState AdventureState { get; set; }

        public Adventure()
        {
            AdventureState = AdventureState.CREATION;
        }

        public abstract void NextStep(Shift shift);
    }

    public class TestRoom : Adventure
    {
        private SimpleFight simpleFight;

        public TestRoom(SimpleFight simpleFight)
        {
            this.simpleFight = simpleFight;
        }

        public override void NextStep(Shift shift)
        {
            AdventureState = AdventureState.RUNNING;

            if (!simpleFight.Run())
            {
                AdventureState = AdventureState.FAILED;
                return;
            }

            AdventureState = AdventureState.COMPLETED;
        }
    }
}
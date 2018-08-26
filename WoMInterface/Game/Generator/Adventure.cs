using WoMInterface.Game.Combat;
using WoMInterface.Game.Interaction;
using WoMInterface.Game.Model;

namespace WoMInterface.Game.Generator
{
    public abstract class Adventure
    {
        public AdventureState AdventureState { get; set; }

        public bool IsActive => AdventureState == AdventureState.CREATION || AdventureState == AdventureState.RUNNING;

        public Adventure()
        {
            AdventureState = AdventureState.CREATION;
        }

        public abstract void NextStep(Mogwai mogwai, Shift shift);
    }

    public class TestRoom : Adventure
    {
        private SimpleFight simpleFight;

        public TestRoom(SimpleFight simpleFight)
        {
            this.simpleFight = simpleFight;
        }

        public override void NextStep(Mogwai mogwai, Shift shift)
        {
            if (AdventureState == AdventureState.CREATION)
            {
                simpleFight.Create(mogwai, shift);
                AdventureState = AdventureState.RUNNING;
            }
            
            if (!simpleFight.Run())
            {
                AdventureState = AdventureState.FAILED;
                return;
            }

            AdventureState = AdventureState.COMPLETED;
        }
    }
}
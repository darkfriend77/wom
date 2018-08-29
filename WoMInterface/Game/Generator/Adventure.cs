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
}
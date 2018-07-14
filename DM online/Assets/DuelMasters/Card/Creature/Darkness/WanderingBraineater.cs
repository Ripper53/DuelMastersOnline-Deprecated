
namespace DuelMasters.Cards {
    public sealed class WanderingBraineater : Creature {
        public override string OriginalName => "Wandering Braineater";

        public WanderingBraineater(Duelist owner) : base(owner) {
            ManaCost = 2;
            Power = 2000;
            Add(Civilization.DARKNESS);
            Add(Race.LIVING_DEAD);

            Blocker = true;
            CanAttack = (creature) => false;
        }

    }
}


namespace DuelMasters.Cards {
    public sealed class Stonesaur : Creature {
        public override string OriginalName => "Stonesaur";

        public Stonesaur(Duelist owner) : base(owner) {
            ManaCost = 5;
            Power = 4000;
            Add(Civilization.FIRE);
            Add(Race.ROCK_BEAST);

            Effect.PowerAttacker(this, 2000);
        }

    }
}

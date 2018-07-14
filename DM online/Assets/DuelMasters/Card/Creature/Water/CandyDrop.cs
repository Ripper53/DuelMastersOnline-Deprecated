
namespace DuelMasters.Cards {
    public sealed class CandyDrop : Creature {
        public override string OriginalName => "Candy Drop";

        public CandyDrop(Duelist owner) : base(owner) {
            ManaCost = 3;
            Power = 1000;
            Add(Civilization.WATER);
            Add(Race.CYBER_VIRUS);

            Blockable = (blocker) => false;
        }

    }
}

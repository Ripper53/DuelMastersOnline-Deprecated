
namespace DuelMasters.Cards {
    public sealed class FaerieChild : Creature {
        public override string OriginalName => "Faerie Child";

        public FaerieChild(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 2000;
            Add(Civilization.WATER);
            Add(Race.CYBER_VIRUS);

            Blockable = (blocker) => false;
        }

    }
}

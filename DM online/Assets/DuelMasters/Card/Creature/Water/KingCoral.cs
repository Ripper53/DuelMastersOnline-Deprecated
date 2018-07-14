
namespace DuelMasters.Cards {
    public sealed class KingCoral : Creature {
        public override string OriginalName => "King Coral";

        public KingCoral(Duelist owner) : base(owner) {
            ManaCost = 3;
            Power = 1000;
            Add(Civilization.WATER);
            Add(Race.LEVIATHAN);

            Blocker = true;
        }

    }
}

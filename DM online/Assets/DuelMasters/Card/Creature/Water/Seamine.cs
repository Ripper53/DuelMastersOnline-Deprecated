
namespace DuelMasters.Cards {
    public sealed class Seamine : Creature {
        public override string OriginalName => "Seamine";

        public Seamine(Duelist owner) : base(owner) {
            ManaCost = 6;
            Power = 4000;
            Add(Civilization.WATER);
            Add(Race.FISH);

            Blocker = true;
        }

    }
}

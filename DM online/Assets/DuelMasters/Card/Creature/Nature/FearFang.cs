
namespace DuelMasters.Cards {
    public sealed class FearFang : Creature {
        public override string OriginalName => "Fear Fang";

        public FearFang(Duelist owner) : base(owner) {
            ManaCost = 3;
            Power = 3000;
            Add(Civilization.NATURE);
            Add(Race.BEAST_FOLK);
        }

    }
}

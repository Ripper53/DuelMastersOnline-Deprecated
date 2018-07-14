
namespace DuelMasters.Cards {
    public sealed class TrihornShepherd : Creature {
        public override string OriginalName => "Tri-horn Shepherd";

        public TrihornShepherd(Duelist owner) : base(owner) {
            ManaCost = 5;
            Power = 5000;
            Add(Civilization.NATURE);
            Add(Race.HORNED_BEAST);
        }

    }
}

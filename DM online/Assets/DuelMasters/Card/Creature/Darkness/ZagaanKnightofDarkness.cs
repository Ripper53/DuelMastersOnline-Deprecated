
namespace DuelMasters.Cards {
    public sealed class ZagaanKnightofDarkness : Creature {
        public override string OriginalName => "Zagaan, Knight of Darkness";

        public ZagaanKnightofDarkness(Duelist owner) : base(owner) {
            ManaCost = 6;
            ShieldBreaker = 2;
            Power = 7000;
            Add(Civilization.DARKNESS);
            Add(Race.DEMON_COMMAND);
        }

    }
}

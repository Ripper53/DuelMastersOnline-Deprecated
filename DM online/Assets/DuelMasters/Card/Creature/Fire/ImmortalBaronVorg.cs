
namespace DuelMasters.Cards {
    public sealed class ImmortalBaronVorg : Creature {
        public override string OriginalName => "Immortal Baron, Vorg";

        public ImmortalBaronVorg(Duelist owner) : base(owner) {
            ManaCost = 2;
            Power = 2000;
            Add(Civilization.FIRE);
            Add(Race.HUMAN);
        }

    }
}

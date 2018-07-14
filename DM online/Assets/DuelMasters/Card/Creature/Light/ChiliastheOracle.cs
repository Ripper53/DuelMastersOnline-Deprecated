
namespace DuelMasters.Cards {
    public sealed class ChiliastheOracle : Creature {
        public override string OriginalName => "Chilias, the Oracle";

        public ChiliastheOracle(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 2500;
            Add(Civilization.LIGHT);
            Add(Race.LIGHT_BRINGER);

            Destroy = () => owner.Hand.Put(this);
        }

    }
}

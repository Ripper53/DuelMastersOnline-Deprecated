
namespace DuelMasters.Cards {
    public sealed class AquaKnight : Creature {
        public override string OriginalName => "Aqua Knight";

        public AquaKnight(Duelist owner) : base(owner) {
            ManaCost = 5;
            Power = 4000;
            Add(Civilization.WATER);
            Add(Race.LIQUID_PEOPLE);

            Destroy = () => Owner.Hand.Put(this);
        }

    }
}

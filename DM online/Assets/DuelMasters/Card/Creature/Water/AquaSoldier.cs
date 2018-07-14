
namespace DuelMasters.Cards {
    public sealed class AquaSoldier : Creature {
        public override string OriginalName => "Aqua Soldier";

        public AquaSoldier(Duelist owner) : base(owner) {
            ManaCost = 3;
            Power = 1000;
            Add(Civilization.WATER);
            Add(Race.LIQUID_PEOPLE);

            Destroy = () => Owner.Hand.Put(this);
        }

    }
}

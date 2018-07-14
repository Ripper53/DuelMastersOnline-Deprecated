
namespace DuelMasters.Cards {
    public sealed class AquaVehicle : Creature {
        public override string OriginalName => "Aqua Vehicle";

        public AquaVehicle(Duelist owner) : base(owner) {
            ManaCost = 2;
            Power = 1000;
            Add(Civilization.WATER);
            Add(Race.LIQUID_PEOPLE);
        }

    }
}

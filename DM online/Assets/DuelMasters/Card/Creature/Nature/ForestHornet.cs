
namespace DuelMasters.Cards {
    public sealed class ForestHornet : Creature {
        public override string OriginalName => "Forest Hornet";

        public ForestHornet(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 4000;
            Add(Civilization.NATURE);
            Add(Race.GIANT_INSECT);
        }

    }
}

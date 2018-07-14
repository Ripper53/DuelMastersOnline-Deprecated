
namespace DuelMasters.Cards {
    public sealed class Gigagiele : Creature {
        public override string OriginalName => "Gigagiele";

        public Gigagiele(Duelist owner) : base(owner) {
            ManaCost = 5;
            Add(Civilization.DARKNESS);
            Add(Race.CHIMERA);

            Effect.Slayer(this);
        }

    }
}

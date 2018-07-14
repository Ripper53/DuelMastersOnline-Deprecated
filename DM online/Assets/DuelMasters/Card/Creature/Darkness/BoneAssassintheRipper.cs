
namespace DuelMasters.Cards {
    public sealed class BoneAssassintheRipper : Creature {
        public override string OriginalName => "Bone Assassin, the Ripper";

        public BoneAssassintheRipper(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 2000;
            Add(Civilization.DARKNESS);
            Add(Race.LIVING_DEAD);

            Effect.Slayer(this);
        }

    }
}

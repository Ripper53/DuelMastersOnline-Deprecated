
namespace DuelMasters.Cards {
    public sealed class WrithingBoneGhoul : Creature {
        public override string OriginalName => "Writhing Bone Ghoul";

        public WrithingBoneGhoul(Duelist owner) : base(owner) {
            ManaCost = 2;
            Power = 2000;
            Add(Civilization.DARKNESS);
            Add(Race.LIVING_DEAD);
        }

    }
}

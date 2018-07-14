
namespace DuelMasters.Cards {
    public sealed class SkeletonSoldiertheDefiled : Creature {
        public override string OriginalName => "Skeleton Soldier, the Defiled";

        public SkeletonSoldiertheDefiled(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 3000;
            Add(Civilization.DARKNESS);
            Add(Race.LIVING_DEAD);
        }

    }
}

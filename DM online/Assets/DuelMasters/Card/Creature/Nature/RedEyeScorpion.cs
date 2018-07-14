
namespace DuelMasters.Cards {
    public sealed class RedEyeScorpion : Creature {
        public override string OriginalName => "Red-Eye Scorpion";

        public RedEyeScorpion(Duelist owner) : base(owner) {
            ManaCost = 5;
            Power = 4000;
            Add(Civilization.NATURE);
            Add(Race.GIANT_INSECT);

            Destroy = () => Owner.ManaZone.Put(this);
        }

    }
}

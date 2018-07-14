
namespace DuelMasters.Cards {
    public sealed class BoneSpider : Creature {
        public override string OriginalName => "Bone Spider";

        public BoneSpider(Duelist owner) : base(owner) {
            ManaCost = 3;
            Power = 5000;
            Add(Civilization.DARKNESS);
            Add(Race.LIVING_DEAD);

            Effect.DestroyAfterWinningBattle(this);
        }

    }
}

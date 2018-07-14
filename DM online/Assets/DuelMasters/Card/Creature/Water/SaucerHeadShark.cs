
namespace DuelMasters.Cards {
    public sealed class SaucerHeadShark : Creature {
        public override string OriginalName => "Saucer-Head Shark";

        public SaucerHeadShark(Duelist owner) : base(owner) {
            ManaCost = 5;
            Power = 3000;
            Add(Civilization.WATER);
            Add(Race.GEL_FISH);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    Owner.TaskList.AddTask(Effect.BounceAllWithLessOrEqualPowerTask(this, 2000, false));
            };
        }

    }
}

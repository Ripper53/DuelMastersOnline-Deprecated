
namespace DuelMasters.Cards {
    public sealed class StingerWorm : Creature {
        public override string OriginalName => "Stinger Worm";

        public StingerWorm(Duelist owner) : base(owner) {
            ManaCost = 3;
            Power = 5000;
            Add(Civilization.DARKNESS);
            Add(Race.PARASITE_WORM);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    Owner.TaskList.AddTask(Effect.DestroyFriendlyTask(this, 1, false));
            };
        }

    }
}

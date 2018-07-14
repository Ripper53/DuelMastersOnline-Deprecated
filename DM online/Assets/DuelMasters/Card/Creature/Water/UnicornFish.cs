
namespace DuelMasters.Cards {
    public sealed class UnicornFish : Creature {
        public override string OriginalName => "Unicorn Fish";

        public UnicornFish(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 1000;
            Add(Civilization.WATER);
            Add(Race.FISH);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    Owner.TaskList.AddTask(Effect.BounceTask<Creature>(this));
            };
        }

    }
}

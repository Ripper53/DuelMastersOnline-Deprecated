
namespace DuelMasters.Cards {
    public sealed class PoisonousMushroom : Creature {
        public override string OriginalName => "Poisonous Mushroom";

        public PoisonousMushroom(Duelist owner) : base(owner) {
            ManaCost = 2;
            Power = 1000;
            Add(Civilization.NATURE);
            Add(Race.BALLOON_MUSHROOM);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    Owner.TaskList.AddTask(Effect.HandToManaZoneTask<Card>(this));
            };
        }

    }
}


namespace DuelMasters.Cards {
    public sealed class RothustheTraveler : Creature {
        public override string OriginalName => "Rothus, the Traveler";

        public RothustheTraveler(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 4000;
            Add(Civilization.FIRE);
            Add(Race.ARMORLOID);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this) {
                    Owner.TaskList.AddTask(Effect.DestroyFriendlyTask(this, 1, false));
                    Effect.OppDestroyTask(this);
                }
            };
        }

    }
}


namespace DuelMasters.Cards {
    public sealed class ThornyMandra : Creature {
        public override string OriginalName => "Thorny Mandra";

        public ThornyMandra(Duelist owner) : base(owner) {
            ManaCost = 5;
            Power = 4000;
            Add(Civilization.NATURE);
            Add(Race.TREE_FOLK);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    Owner.TaskList.AddTask(Effect.GraveyardToManaZoneFriendlyTask<Card>(this));
            };
        }

    }
}

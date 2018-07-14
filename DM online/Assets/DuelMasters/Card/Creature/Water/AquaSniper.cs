
namespace DuelMasters.Cards {
    public sealed class AquaSniper : Creature {
        public override string OriginalName => "Aqua Sniper";

        public AquaSniper(Duelist owner) : base(owner) {
            ManaCost = 8;
            Power = 5000;
            Add(Civilization.WATER);
            Add(Race.LIQUID_PEOPLE);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this) owner.TaskList.AddTask(Effect.BounceTask<Creature>(this, 2));
            };
        }

    }
}

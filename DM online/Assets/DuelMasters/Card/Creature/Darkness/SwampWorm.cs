
namespace DuelMasters.Cards {
    public sealed class SwampWorm : Creature {
        public override string OriginalName => "Swamp Worm";

        public SwampWorm(Duelist owner) : base(owner) {
            ManaCost = 7;
            Power = 2000;
            Add(Civilization.DARKNESS);
            Add(Race.PARASITE_WORM);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    Effect.OppDestroyTask(this);
            };
        }

    }
}

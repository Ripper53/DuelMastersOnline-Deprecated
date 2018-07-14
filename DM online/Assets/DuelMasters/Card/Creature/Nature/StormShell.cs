
namespace DuelMasters.Cards {
    public sealed class StormShell : Creature {
        public override string OriginalName => "Storm Shell";

        public StormShell(Duelist owner) : base(owner) {
            ManaCost = 7;
            Power = 2000;
            Add(Civilization.NATURE);
            Add(Race.COLONY_BEETLE);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    Effect.OppBattleZoneToManaZoneTask<Card>(this);
            };
        }

    }
}

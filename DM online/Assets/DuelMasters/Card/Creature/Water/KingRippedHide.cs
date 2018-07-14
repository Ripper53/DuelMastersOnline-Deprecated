
namespace DuelMasters.Cards {
    public sealed class KingRippedHide : Creature {
        public override string OriginalName => "King Ripped-Hide";

        public KingRippedHide(Duelist owner) : base(owner) {
            ManaCost = 7;
            Power = 5000;
            Add(Civilization.WATER);
            Add(Race.LEVIATHAN);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this) Effect.DrawUpTo(this, 2);
            };
        }

    }
}

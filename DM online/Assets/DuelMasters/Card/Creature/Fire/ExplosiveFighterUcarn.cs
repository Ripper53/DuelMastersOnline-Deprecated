
namespace DuelMasters.Cards {
    public sealed class ExplosiveFighterUcarn : Creature {
        public override string OriginalName => "Explosive Fighter Ucarn";

        public ExplosiveFighterUcarn(Duelist owner) : base(owner) {
            ManaCost = 5;
            ShieldBreaker = 2;
            Power = 9000;
            Add(Civilization.FIRE);
            Add(Race.DRAGONOID);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    Owner.TaskList.AddTask(Effect.ManaZoneToGraveyardTask<Card>(this, 2, false));
            };
        }

    }
}

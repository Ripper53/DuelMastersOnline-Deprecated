
namespace DuelMasters.Cards {
    public sealed class ArtisanPicora : Creature {
        public override string OriginalName => "Artisan Picora";

        public ArtisanPicora(Duelist owner) : base(owner) {
            ManaCost = 1;
            Power = 2000;
            Add(Civilization.FIRE);
            Add(Race.MACHINE_EATER);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    Owner.TaskList.AddTask(Effect.ManaZoneToGraveyardTask<Card>(this, 1, false));
            };
        }

    }
}

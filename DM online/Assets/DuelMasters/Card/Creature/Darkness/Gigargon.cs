
namespace DuelMasters.Cards {
    public sealed class Gigargon : Creature {
        public override string OriginalName => "Gigargon";

        public Gigargon(Duelist owner) : base(owner) {
            ManaCost = 8;
            Power = 3000;
            Add(Civilization.DARKNESS);
            Add(Race.CHIMERA);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    Owner.TaskList.AddTask(Effect.ReturnFromGraveyardTask<Creature>(this, 2));
            };
        }

    }
}

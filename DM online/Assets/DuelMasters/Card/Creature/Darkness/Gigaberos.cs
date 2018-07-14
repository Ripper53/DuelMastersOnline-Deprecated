
namespace DuelMasters.Cards {
    public sealed class Gigaberos : Creature {
        public override string OriginalName => "Gigaberos";

        public Gigaberos(Duelist owner) : base(owner) {
            ManaCost = 5;
            Power = 8000;
            ShieldBreaker = 2;
            Add(Civilization.DARKNESS);
            Add(Race.CHIMERA);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    Owner.TaskList.AddTask(Effect.DestroySelfOrOtherTask(this, 2));
            };
        }

    }
}

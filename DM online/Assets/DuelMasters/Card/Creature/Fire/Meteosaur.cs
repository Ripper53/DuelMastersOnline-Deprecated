
namespace DuelMasters.Cards {
    public sealed class Meteosaur : Creature {
        public override string OriginalName => "Meteosaur";

        public Meteosaur(Duelist owner) : base(owner) {
            ManaCost = 5;
            Power = 2000;
            Add(Civilization.FIRE);
            Add(Race.ROCK_BEAST);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    Owner.TaskList.AddTask(Effect.DestroyPowerLessOrEqualOppTask(this, 2000));
            };
        }

    }
}

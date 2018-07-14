
namespace DuelMasters.Cards {
    public sealed class VampireSilphy : Creature {
        public override string OriginalName => "Vampire Silphy";

        public VampireSilphy(Duelist owner) : base(owner) {
            ManaCost = 8;
            Power = 4000;
            Add(Civilization.DARKNESS);
            Add(Race.DARK_LORD);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    Owner.TaskList.AddTask(Effect.DestroyAllWithPowerLessOrEqualToTask(this, 3000, false));
            };
        }

    }
}

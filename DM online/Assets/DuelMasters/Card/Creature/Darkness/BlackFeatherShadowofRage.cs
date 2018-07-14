
namespace DuelMasters.Cards {
    public sealed class BlackFeatherShadowofRage : Creature {
        public override string OriginalName => "Black Feather, Shadow of Rage";

        public BlackFeatherShadowofRage(Duelist owner) : base(owner) {
            ManaCost = 1;
            Power = 3000;
            Add(Civilization.DARKNESS);
            Add(Race.GHOST);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    Owner.TaskList.AddTask(Effect.DestroyFriendlyTask(this, 1, false));
            };
        }

    }
}

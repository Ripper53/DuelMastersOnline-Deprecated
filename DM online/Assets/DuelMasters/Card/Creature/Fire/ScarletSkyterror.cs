
namespace DuelMasters.Cards {
    public sealed class ScarletSkyterror : Creature {
        public override string OriginalName => "Scarlet Skyterror";

        public ScarletSkyterror(Duelist owner) : base(owner) {
            ManaCost = 8;
            Power = 3000;
            Add(Civilization.FIRE);
            Add(Race.ARMORED_WYVERN);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    owner.TaskList.AddTask(Effect.DestroyAllBlockersTask(this, false));
            };
        }

    }
}


namespace DuelMasters.Cards {
    public sealed class BronzeArmTribe : Creature {
        public override string OriginalName => "Bronze-Arm Tribe";

        public BronzeArmTribe(Duelist owner) : base(owner) {
            ManaCost = 3;
            Power = 1000;
            Add(Civilization.NATURE);
            Add(Race.BEAST_FOLK);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    Owner.TaskList.AddTask(Effect.TopCardOfDeckIntoManaZoneTask(this, 1, false));
            };
        }

    }
}


namespace DuelMasters.Cards {
    public sealed class RaylaTruthEnforcer : Creature {
        public override string OriginalName => "Rayla, Truth Enforcer";

        public RaylaTruthEnforcer(Duelist owner) : base(owner) {
            ManaCost = 6;
            Power = 3000;
            Add(Civilization.LIGHT);
            Add(Race.BERSERKER);

            owner.Game.BattleZonePut += (game, bz, putCard) => {
                if (putCard == this)
                    Owner.TaskList.AddTask(Effect.SearchDeckForTask<Spell>(this));
            };
        }

    }
}

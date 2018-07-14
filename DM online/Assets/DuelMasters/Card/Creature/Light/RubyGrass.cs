
namespace DuelMasters.Cards {
    public sealed class RubyGrass : Creature {
        public override string OriginalName => "Ruby Grass";

        public RubyGrass(Duelist owner) : base(owner) {
            ManaCost = 3;
            Power = 3000;
            Add(Civilization.LIGHT);
            Add(Race.STARLIGHT_TREE);

            Blocker = true;
            CanAttackPlayer = (player) => false;
            owner.AtTurnEnd += (duelist) => {
                if (Tapped)
                    owner.TaskList.AddTask(Effect.UntapSelfTask(this));
            };
        }

    }
}

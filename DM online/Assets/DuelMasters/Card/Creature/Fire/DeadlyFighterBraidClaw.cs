
namespace DuelMasters.Cards {
    public sealed class DeadlyFighterBraidClaw : Creature {
        public override string OriginalName => "Deadly Fighter Braid Claw";

        public DeadlyFighterBraidClaw(Duelist owner) : base(owner) {
            ManaCost = 1;
            Power = 1000;
            Add(Civilization.FIRE);
            Add(Race.DRAGONOID);

            Effect.AttackEachTurnIfAble(this);
        }

    }
}


namespace DuelMasters.Cards {
    public sealed class Draglide : Creature {
        public override string OriginalName => "Draglide";

        public Draglide(Duelist owner) : base(owner) {
            ManaCost = 5;
            Power = 5000;
            Add(Civilization.FIRE);
            Add(Race.ARMORED_WYVERN);

            Effect.AttackEachTurnIfAble(this);
        }

    }
}

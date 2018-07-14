
namespace DuelMasters.Cards {
    public sealed class FatalAttackerHorvath : Creature {
        public override string OriginalName => "Fatal Attacker Horvath";

        public FatalAttackerHorvath(Duelist owner) : base(owner) {
            ManaCost = 3;
            Power = 2000;
            Add(Civilization.FIRE);
            Add(Race.HUMAN);

            Effect.PowerAttacker(this, 2000, () => Owner.BattleZone.Contains(Race.ARMORLOID));
        }

    }
}

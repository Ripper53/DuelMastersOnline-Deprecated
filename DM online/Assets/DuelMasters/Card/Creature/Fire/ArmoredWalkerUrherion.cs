
namespace DuelMasters.Cards {
    public sealed class ArmoredWalkerUrherion : Creature {
        public override string OriginalName => "Armored Walker Urherion";

        public ArmoredWalkerUrherion(Duelist owner) : base(owner) {
            ManaCost = 4;
            Power = 3000;
            Add(Civilization.FIRE);
            Add(Race.ARMORLOID);

            Effect.PowerAttacker(this, 2000, () => Owner.BattleZone.Contains(Race.HUMAN));
        }

    }
}

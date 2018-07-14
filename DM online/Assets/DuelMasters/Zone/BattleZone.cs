
namespace DuelMasters {
    public sealed class BattleZone : Zone {

        public BattleZone(Duelist owner) : base(owner) { }

        protected override void OnPutCard(Card putCard) {
            if (putCard is Creature) {
                Creature putCreature = (Creature)putCard;
                putCreature.SummoningSickness = true;
            }
            base.OnPutCard(putCard);
        }

    }
}

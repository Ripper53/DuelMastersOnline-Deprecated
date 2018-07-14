
namespace DuelMasters {
    public sealed class Hand : Zone {

        public Hand(Duelist owner) : base(owner) { }

        public void Discard(int index) {
            Owner.Graveyard.Put(this[index]);
        }

    }
}

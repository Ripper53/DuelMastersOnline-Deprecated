
namespace DuelMasters {
    public sealed class ShieldZone : Zone {

        public ShieldZone(Duelist owner) : base(owner) { }

        public void BreakShield(int index) {
            BreakShield(index, Owner.Hand);
        }

        public void BreakShield(int index, Zone zoneToPutShieldIn) {
            BreakShield(this[index], zoneToPutShieldIn);
        }

        public void BreakShield(Card shield) {
            BreakShield(shield, Owner.Hand);
        }

        public void BreakShield(Card shield, Zone zoneToPutShieldIn) {
            zoneToPutShieldIn.Put(shield);
            OnShieldBroken(shield, zoneToPutShieldIn);
            if (shield.ShieldTrigger && zoneToPutShieldIn is Hand) {
                shield.Owner.TaskList.AddTask(new DuelTask(
                    $"{shield.Name}: Shield Trigger.",
                    (args) => shield.Use()
                ));
            }
        }

        public delegate void ShieldBrokenEventHandler(ShieldZone source, Card brokenShield, Zone zoneShieldPutIn);
        public event ShieldBrokenEventHandler ShieldBroken;
        private void OnShieldBroken(Card brokenShield, Zone zoneShieldPutIn) {
            ShieldBroken?.Invoke(this, brokenShield, zoneShieldPutIn);
        }

    }
}

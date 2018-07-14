using System.Collections.Generic;

namespace DuelMasters {
    public class CreatureAction {
        public Creature Creature { get; private set; }

        public List<Creature> CanAttackUntapped { get; private set; }

        public CreatureAction(Creature creature) {
            Creature = creature;
            CanAttackUntapped = new List<Creature>();
        }

        public bool EvalCanAttack(Creature defendingCreature) {
            bool check = !(
                Creature.SummoningSickness ||
                Creature.Tapped ||
                !Creature.CanAttack(defendingCreature)
            );
            if (defendingCreature != null)
                return check && (defendingCreature.Tapped || Creature.CanAttackUntapped(defendingCreature) || CanAttackUntapped.Contains(defendingCreature));
            else
                return check;
        }
        public bool EvalCanAttackPlayer(Duelist defendingPlayer) {
            return EvalCanAttack(null) && Creature.CanAttackPlayer(defendingPlayer);
        }

    }
}

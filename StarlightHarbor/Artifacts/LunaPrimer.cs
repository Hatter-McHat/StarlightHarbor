using System.Collections.Generic;
using StarlightHarbor.Cards;

namespace StarlightHarbor.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] { ArtifactPool.Boss }, unremovable = true)]
    internal class LunaPrimer: Artifact
    {

        private int casingsSpent = 0;
        private int casingsLimit = 5;
        //private bool casingRefund = true;
        public override string Description() => "";
        public override int? GetDisplayNumber(State s) => casingsSpent;
        public override void OnPlayerPlayCard(
            int energyCost,
            Deck deck,
            Card card,
            State state,
            Combat combat,
            int handPosition,
            int handCount)
        {
            //if card is casing, incrmnt
            //if 5, +pwr
            /*
            if (card.Name() == "Spent Casings")
            {
                casingsSpent++;
                if (casingRefund) {
                    AEnergy a = new AEnergy();
                    a.changeAmount = 1;
                    ADrawCard b = new ADrawCard();
                    b.count = 1;
                    b.artifactPulse = this.Key();
                    combat.QueueImmediate((CardAction)a);
                    combat.QueueImmediate((CardAction)b);
                    casingRefund = false;
                }
            }
            */
            if (card.Name() == "Spent Casings") 
            {
                casingsSpent++;
            }

            if (casingsSpent >= casingsLimit)
            {
 
                Combat combat1 = combat;
                AStatus a = new AStatus();
                a.status = Status.powerdrive;
                a.statusAmount = 1;
                a.targetPlayer = true;
                a.artifactPulse = this.Key();
                combat1.QueueImmediate((CardAction)a);
                casingsSpent -= casingsLimit;
            }
            
        }
        //public override void OnTurnEnd(State state, Combat combat) => casingRefund = true;

    }
}

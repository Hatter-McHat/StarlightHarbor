using System.Collections.Generic;
using StarlightHarbor.Cards;

namespace StarlightHarbor.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] { ArtifactPool.Boss }, unremovable = true)]
    internal class MuniStrike : Artifact
    {
        private const int drawPenalty = 3;
        private bool activated = false;

        public override string Description() => "Draw 3 less cards per turn, on first card play card an additional time and draw 3 cards";

        //hmmm ZeroDoubler
        public override void OnPlayerPlayCard(
            int energyCost,
            Deck deck,
            Card card,
            State state,
            Combat combat,
            int handPosition,
            int handCount)
            {
                if (activated)
                    return;
                activated = true;
                this.Pulse();
            combat.Queue((CardAction)new ADrawCard()
            {
                count = drawPenalty
            });
            combat.Queue((CardAction)new MuniADoublerTheOtherDirection()
                {
                    uuid = card.uuid,
                    backupCard = card
                });
            }

        public override void OnTurnStart(State state, Combat combat) => activated = false;

        public override void OnReceiveArtifact(State state)
        {
            //hmmm yes, coding
            state.ship.baseDraw -= drawPenalty;
        }

        public override void OnRemoveArtifact(State state)
        {
            state.ship.baseEnergy += drawPenalty;
        }

    }
}

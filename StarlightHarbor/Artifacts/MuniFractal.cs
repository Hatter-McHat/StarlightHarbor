using System.Collections.Generic;
using StarlightHarbor.Cards;

namespace StarlightHarbor.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] { ArtifactPool.EventOnly }, unremovable = true)]
    internal class MuniFractal : Artifact
    {
        private const int usesPerTurn = 2;
        private int uses = 0;

        public override string Description() => "";
        public override int? GetDisplayNumber(State s) => new int?(usesPerTurn - uses);

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
                if (uses >= usesPerTurn)
                    return;
                uses++;
                this.Pulse();
                combat.Queue((CardAction)new MuniADoubler()
                {
                    uuid = card.uuid,
                    backupCard = card
                });
            }

        public override void OnTurnStart(State state, Combat combat) => uses = 0;

        public override void OnReceiveArtifact(State state)
        {
            //hmmm yes, coding
            state.ship.baseEnergy--;
        }

        public override void OnRemoveArtifact(State state)
        {
            state.ship.baseEnergy++;
        }

    }
}

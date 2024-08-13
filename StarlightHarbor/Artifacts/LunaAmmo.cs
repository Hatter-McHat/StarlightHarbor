using System.Collections.Generic;
using StarlightHarbor.Cards;

namespace StarlightHarbor.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] { ArtifactPool.EventOnly }, unremovable = true)]
    internal class LunaAmmo : Artifact
    {
        public bool generateCasing = false;
        public int count = 0;
        public int energyStore = 2;
        public override string Description() => "Gain 2 less <c=ENERGY</c> every turn.</c> The first 2 times you play a card each turn, gain 1 <c=energy>ENERGY</c>.\n<c=downside>Whenever you play an <c=action>attacking</c> card, add 1 <c=card>Spent Casings</c> to your draw pile.</c>";
        public override int? GetDisplayNumber(State s) => new int?(energyStore - this.count);
        //if a card fires the cannon, add a spent casings.
        //just trash right now

        public override void OnPlayerPlayCard(
            int energyCost,
            Deck deck,
            Card card,
            State state,
            Combat combat,
            int handPosition,
            int handCount)
        {
            //if card has an attack action
            //add casing
            // combat.QueueImmediatee((CardAction)new AAddCard()
            if (card.GetActions(state, combat).Any(a => a is AAttack))
            {
                generateCasing = true;
            }
            else { 
                generateCasing = false; 
            }
            if (this.count >= this.energyStore)
                return;
            ++this.count;
            //add energy
            AEnergy a = new AEnergy();
            a.changeAmount = 1;
            a.artifactPulse = this.Key();
            combat.QueueImmediate((CardAction)a);
        }
        public override void OnPlayerAttack(State state, Combat combat)
        {
            if (generateCasing) {
                combat.Queue((CardAction)new AAddCard()
                {
                    card = (Card)new LunaCasingsCard(),
                    destination = CardDestination.Deck
                });
                generateCasing = false;
            }

        }
        //base energy manipulation
        public override void OnReceiveArtifact(State state)
        {
            //hmmm yes, coding
            state.ship.baseEnergy-= energyStore;
            //add cards
            Card CannonA = (Card)new CannonColorless();
            Card DodgeA = (Card)new DodgeColorless();
            CannonA.upgrade = Upgrade.A;
            DodgeA.upgrade = Upgrade.A;
            state.SendCardToDeck(CannonA, insertRandomly: true);
            state.SendCardToDeck(DodgeA, insertRandomly: true);
        }

        public override void OnRemoveArtifact(State state)
        {
            state.ship.baseEnergy+= energyStore;
        }
        public override List<Tooltip>? GetExtraTooltips()
        {
            List<Tooltip> extraTooltips = new List<Tooltip>();
            extraTooltips.Add((Tooltip)new TTCard()
            {
                card = (Card)new LunaCasingsCard()
            });
            //extraTooltips.Add((Tooltip)new TTGlossary("cardtrait.retain", Array.Empty<object>()));
            //extraTooltips.Add((Tooltip)new TTGlossary("cardtrait.flippable", Array.Empty<object>()));
            return extraTooltips;
        }
        public override void OnTurnEnd(State state, Combat combat) => this.count = 0;

        public override void OnCombatEnd(State state) => this.count = 0;

    }
}

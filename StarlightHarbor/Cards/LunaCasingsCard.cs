
using System.Collections.Generic;

#nullable enable

namespace StarlightHarbor.Cards
{   
    [CardMeta(deck = Deck.trash, rarity = Rarity.common, dontOffer = true)]
    internal class LunaCasingsCard : Card
    {
        public override string Name() => "Spent Casings";

        public override CardData GetData(State state) => new CardData()
        {
            cost = 1,
            singleUse = true,
            //art = new Spr?(Spr.cards_Trash)         
        };

        public override List<CardAction> GetActions(State s, Combat c) => new List<CardAction>();
    }

}

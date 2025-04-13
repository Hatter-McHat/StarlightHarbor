using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using CobaltCoreModding.Definitions.ModContactPoints;
using Shockah.Kokoro;
using StarlightHarbor.Mod;
using static System.Collections.Specialized.BitVector32;

namespace StarlightHarbor.Cards
{
    //from Soggins by Shockah
    internal class MuniADoublerTheOtherDirection : CardAction
    {
        public int uuid;
        public Card backupCard;
        //private List<CardAction> actions = new List<CardAction>();
        private static StarlightHarbor.Mod.ModEntry Instance => StarlightHarbor.Mod.ModEntry.Instance;
        //internal IKokoroApi KokoroApi { get; private set; } = null!;

        public override void Begin(G g, State state, Combat combat)
        {           
            List<CardAction> actions = new List<CardAction>();
            this.timer = 0.0;
            Card card = state.FindCard(this.uuid) ?? this.backupCard;
            var toAdd = card.GetActionsOverridden(state, combat)
                        .Where(a => a is not AEndTurn)
                        .ToList();
            
                var isSpawnAction = toAdd.SelectMany(a => Instance.KokoroApi.Actions.GetWrappedCardActionsRecursively(a)).Any(a => a is ASpawn);
            if (isSpawnAction)
            {
                toAdd.Add(new ADroneMove { dir = -1 });
                combat.Queue(new ADroneMove { dir = 1 });
            }
            actions.InsertRange(0, toAdd);
            foreach (CardAction action in actions) {
                combat.Queue(action);           
            }
           
        }
    }
}

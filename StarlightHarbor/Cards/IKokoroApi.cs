using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarlightHarbor
{
    public partial interface IKokoroApi
    {
        IActionApi Actions { get; }

        public interface IActionApi
        {
            List<CardAction> GetWrappedCardActionsRecursively(CardAction action);
        }
    }
}

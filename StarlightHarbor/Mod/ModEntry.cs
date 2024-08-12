
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CobaltCoreModding.Definitions.ModManifests;
using CobaltCoreModding.Definitions;
using Shockah.Kokoro;
using CobaltCoreModding.Definitions.ModContactPoints;
using Microsoft.Extensions.Logging;

namespace StarlightHarbor.Mod
{
    public sealed partial class ModEntry 
    {
        internal static ModEntry Instance { get; private set; } = null!;
        internal IKokoroApi KokoroApi { get; private set; } = null!;
        public void BootMod(IModLoaderContact contact)
        {
            Instance = this;
            KokoroApi = contact.GetApi<IKokoroApi>("Shockah.Kokoro")!;
        }
    }
}


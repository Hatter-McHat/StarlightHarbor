using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Nanoray.PluginManager;
using Nickel;

namespace StarlightHarbor.Mod
{
    internal sealed class MuniPostFix
    {/*
        public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
        {        
            ModEntry.Instance.Harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.Render)),
                postfix: new HarmonyMethod(AccessTools.DeclaredMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Combat_Render_Postfix_Muni)), priority: Priority.First)
            );
         
        }

        private static void Combat_Render_Postfix_Muni(Combat __instance, G g)
        {
            if (__instance.isHoveringEndTurn == 2)
            {
                List<Tooltip> tooltips = new List<Tooltip>();
                int x = g.state.ship.x;
                foreach (Part part in g.state.ship.parts)
                {
                    if (part.type == PType.missiles && part.active)
                    {
                        if (g.state.route is Combat route && route.stuff.ContainsKey(x))
                            route.stuff[x].hilight = 2;
                        part.hilight = true;
                    }
                    ++x;
                }
            }
        }
        */
    }
}

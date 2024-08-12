
using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
//using parchmentArmada.Artifacts;
using StarlightHarbor.Artifacts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;


#nullable enable
namespace StarlightHarbor.Ships
{
    internal class Muni :
      ISpriteManifest,
      IManifest,
      IShipPartManifest,
      IShipManifest,
      IStartershipManifest,
      IArtifactManifest
    {
        public static Dictionary<string, ExternalSprite> sprites = new Dictionary<string, ExternalSprite>();
        public static Dictionary<string, ExternalPart> parts = new Dictionary<string, ExternalPart>();
        public static Dictionary<string, ExternalArtifact> artifacts = new Dictionary<string, ExternalArtifact>();
        private ExternalShip? muni;

        public DirectoryInfo? ModRootFolder { get; set; }


        public string Name => "starlight.harbor.muni";

        public IEnumerable<DependencyEntry> Dependencies => (IEnumerable<DependencyEntry>)new DependencyEntry[0];

        public ILogger? Logger { get; set; }

        //public static ExternalGlossary? ErisStrifeEngineGlossary { get; private set; }

        public DirectoryInfo? GameRootFolder { get; set; }

        private void addSprite(string name, ISpriteRegistry artRegistry)
        {
            if (this.ModRootFolder == null)
                throw new Exception("Root Folder not set");
            string fileName = Path.Combine(this.ModRootFolder.FullName, "Sprites", "Muni", Path.GetFileName(name + ".png"));
            Muni.sprites.Add(name, new ExternalSprite("starlight.harbor.muni." + name, new FileInfo(fileName)));
            artRegistry.RegisterArt(Muni.sprites[name]);
        }

        private void addPart(
          string name,
        string sprite,
          PType type,
          bool flip,
          PDamMod damageMod,
          IShipPartRegistry registry)
        {
            Dictionary<string, ExternalPart> parts = Muni.parts;
            string key = name;
            string globalName = "starlight.harbor.muni." + name;
            Part partObjectTemplate = new Part();
            partObjectTemplate.active = true;
            partObjectTemplate.damageModifier = damageMod;
            partObjectTemplate.type = type;
            partObjectTemplate.flip = flip;
            ExternalPart externalPart = new ExternalPart(globalName, (object)partObjectTemplate, Muni.sprites[sprite] ?? throw new Exception());
            parts.Add(key, externalPart);
            registry.RegisterPart(Muni.parts[name]);
        }

        private void addArtifact(
          string name,
          string art,
          string desc,
          Type artifact,
          IArtifactRegistry registry)
        {
            Muni.artifacts.Add(name, new ExternalArtifact("starlight.harbor" + name, artifact, Muni.sprites[art], (IEnumerable<ExternalGlossary>)new ExternalGlossary[0], (ExternalDeck)null, (IEnumerable<int>)null, (IEnumerable<string>)new string[1]));
            Muni.artifacts[name].AddLocalisation(name, desc);
            registry.RegisterArtifact(Muni.artifacts[name]);
        }

        public void LoadManifest(ISpriteRegistry artRegistry)
        {
            this.addSprite("muni_cockpit", artRegistry);
            this.addSprite("muni_cannon", artRegistry);
            this.addSprite("muni_launcher", artRegistry);
            this.addSprite("muni_wing", artRegistry);
            this.addSprite("muni_structure", artRegistry);
            //non-part
            this.addSprite("muni_chassis", artRegistry);
            this.addSprite("muni_fractal", artRegistry);
            this.addSprite("muni_strike", artRegistry);

        }

        public void LoadManifest(IShipPartRegistry registry)
        {
            //add parts here, flip boolean
            //oh boy, different naming conventions
            this.addPart("wing0", "muni_wing", (PType)3, false, (PDamMod)3, registry);
            this.addPart("cannon", "muni_cannon", (PType)1, false, 0, registry);
            this.addPart("structure0", "muni_structure", (PType)4, false, 0, registry);
            this.addPart("cockpit", "muni_cockpit", (PType)0, false, 0, registry);
            this.addPart("structure1", "muni_structure", (PType)4, true, 0, registry);
            this.addPart("launcher", "muni_launcher", (PType)2, false, 0, registry);
            this.addPart("wing1", "muni_wing", (PType)3, true, (PDamMod)3, registry);
        }

        public void LoadManifest(IArtifactRegistry registry)
        {
            this.addArtifact("MUNI FRACTAL", "muni_fractal", "The first 2 cards you play each turn do their actions twice. <c=downside>Gain 1 less <c=ENERGY</c> every turn.</c>", typeof(MuniFractal), registry);
            this.addArtifact("MUNI STRIKE", "muni_strike", "<c=downside>Draw 3 less cards per turn.</c> When you play your first card each turn, repeat its actions an additional time and draw 3 cards", typeof(MuniStrike), registry);
        }

        public void LoadManifest(IShipRegistry shipRegistry)
        {
            Ship shipObjectTemplate = new Ship();
            shipObjectTemplate.baseDraw = 5;
            shipObjectTemplate.baseEnergy = 3;
            shipObjectTemplate.heatTrigger = 3;
            shipObjectTemplate.heatMin = 0;
            shipObjectTemplate.hull = 8;
            shipObjectTemplate.hullMax = 8;
            shipObjectTemplate.shieldMaxBase = 6;
            //left to right, ship made here
            ExternalPart[] parts = new ExternalPart[7]
            {
                Muni.parts["wing0"],
                Muni.parts["cannon"],
                Muni.parts["structure0"],
                Muni.parts["cockpit"],
                Muni.parts["structure1"],
                Muni.parts["launcher"],
                Muni.parts["wing1"],
            };
            this.muni = new ExternalShip("starlight.harbor.muniShip", (object)shipObjectTemplate, (IEnumerable<ExternalPart>)parts, Muni.sprites["muni_chassis"] ?? throw new Exception());
            shipRegistry.RegisterShip(this.muni);
        }

        public void LoadManifest(IStartershipRegistry registry)
        {
            if (this.muni == null)
                return;
            ExternalStarterShip starterShip = new ExternalStarterShip("starlight.harbor.muniStarter", this.muni.GlobalName, (IEnumerable<ExternalCard>)new ExternalCard[0], (IEnumerable<ExternalArtifact>)new ExternalArtifact[1]
            {
               //change from 0 to 1 when uncommenting
               Muni.artifacts["MUNI FRACTAL"]
            }, (IEnumerable<Type>)new Type[3]
            {
                typeof (DodgeColorless),
                typeof (BasicShieldColorless),
                typeof (CannonColorless)
            }, (IEnumerable<Type>)new Type[1]
            {
                new ShieldPrep().GetType()
            }, (IEnumerable<ExternalArtifact>)new ExternalArtifact[1]
            {
                Muni.artifacts["MUNI STRIKE"]
            });
            starterShip.AddLocalisation(nameof(Muni), "An ancient ship that crackles with unknown power.");
            registry.RegisterStartership(starterShip);
        }
    }
}
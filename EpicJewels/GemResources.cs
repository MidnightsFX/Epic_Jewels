using EpicJewels.Common;
using Jewelcrafting;
using Jotunn.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EpicJewels
{
    internal static class GemResources
    {
        class GemDefition {
            public Color Color { get; set; }
            public Material Material { get; set; }
        }

        static Dictionary<string, GemDefition> GemDefinitions = new Dictionary<string, GemDefition>();

        public static void AddGems()
        {
            Material jade = EpicJewels.EmbeddedResourceBundle.LoadAsset<Material>("gem_jade.mat");
            Color JadeGemColor = new Color(0.031f, 0.69f, 0.043f, 1f);
            GemDefinitions.Add("Jade", new GemDefition() { Color = JadeGemColor, Material = jade });

            Material garnet = EpicJewels.EmbeddedResourceBundle.LoadAsset<Material>("garnet_gem.mat");
            Color GarnetGemColor = new Color(1f, 0.141f, 0.039f, 1f);
            GemDefinitions.Add("Garnet", new GemDefition() { Color = GarnetGemColor, Material = garnet });

            Material amber = EpicJewels.EmbeddedResourceBundle.LoadAsset<Material>("amber_gem.mat");
            Color AmberGemColor = new Color(0.9529412f, 0.7568628f, 0.09803922f, 1f);
            GemDefinitions.Add("Amber", new GemDefition() { Color = AmberGemColor, Material = amber });

            Material opal = EpicJewels.EmbeddedResourceBundle.LoadAsset<Material>("opal_gem.mat");
            Color OpalGemColor = new Color(0.945f, 0.988f, 0.988f, 1f);
            GemDefinitions.Add("Opal", new GemDefition() { Color = OpalGemColor, Material = opal });

            Material amethyst = EpicJewels.EmbeddedResourceBundle.LoadAsset<Material>("amethyst_gem.mat");
            Color AmethystGemColor = new Color(0.784f, 0.302f, 0.98f, 1f);
            GemDefinitions.Add("Amethyst", new GemDefition() { Color = AmethystGemColor, Material = amethyst });

            Material aquamarine = EpicJewels.EmbeddedResourceBundle.LoadAsset<Material>("aquamarine_gem.mat");
            Color AquamarineGemColor = new Color(0.259f, 0.663f, 0.71f, 1f);
            GemDefinitions.Add("Aquamarine", new GemDefition() { Color = AquamarineGemColor, Material = aquamarine });

            AddGemRegisterOverride("Jade");
            AddGemRegisterOverride("Amber");
            AddGemRegisterOverride("Aquamarine");
            AddGemRegisterOverride("Garnet");
            AddGemRegisterOverride("Opal");
            AddGemRegisterOverride("Amethyst");

            API.AddGemConfig(EpicJewels.LoadEmbeddedAssetToString("EJConfig.yaml"));
        }

        // Prefab name -> gem name, for the single material pass below.
        static Dictionary<string, string> GemMaterialOverrides = new Dictionary<string, string>();
        static bool GemMaterialPassRegistered = false;

        internal static void AddGemRegisterOverride(string name) {
            API.AddGems(name, name.ToLower(), GemDefinitions[name].Color);

            // Replace the generated crystals texture with the proper one at runtime
            // This is a workaround to Jewelcrafting choosing to not support HDR.
            // Queued into one pass: each override needs a full Resources.FindObjectsOfTypeAll scan,
            // and subscribing one closure per gem meant six scans of every loaded object.
            GemMaterialOverrides[$"Raw_{name.ToLower()}_Gemstone"] = name;
            if (GemMaterialPassRegistered == false) {
                GemMaterialPassRegistered = true;
                MinimapManager.OnVanillaMapAvailable += ApplyGemMaterials;
            }
        }

        private static void ApplyGemMaterials() {
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>()) {
                string gem_name;
                if (GemMaterialOverrides.TryGetValue(obj.name, out gem_name) == false) { continue; }
                EJLogger.LogDebug($"Applying {gem_name} gem material to {obj.name}.");
                // These gemstones are made up of multiple objects kitbashed together, replace the material of all on this formation.
                obj.GetComponentsInChildren<MeshRenderer>().ToList().ForEach(renderer => renderer.material = GemDefinitions[gem_name].Material);
            }
        }

        internal static void AddAllCrystalLights() {
            AddCrystalLightResources("Jade");
            AddCrystalLightResources("Amber");
            AddCrystalLightResources("Aquamarine");
            AddCrystalLightResources("Garnet");
            AddCrystalLightResources("Opal");
            AddCrystalLightResources("Amethyst");

            // Flush all of the prefabs out and set them up
            JotunnPiece.SetupJotunnPieces();
        }

        internal static void AddCrystalLightResources(string name) {
            // Firebowl
            JotunnPiece.JotunnBuildPiece CrystalBrazierBowl = new JotunnPiece.JotunnBuildPiece();
            CrystalBrazierBowl.Name = $"{name} Brazier";
            CrystalBrazierBowl.Prefab = $"CL_Brazier_{name}";
            CrystalBrazierBowl.Sprite = $"CL_Brazier_{name}";
            CrystalBrazierBowl.Workbench = "forge";
            CrystalBrazierBowl.Category = "Crystal Lights";
            CrystalBrazierBowl.PieceCost = new List<JotunnPiece.PieceCost>() {
                { new JotunnPiece.PieceCost() { prefab = $"Uncut_{name.ToLower()}_Stone", amount = 10, refundable = true } },
                { new JotunnPiece.PieceCost() { prefab = "IronNails", amount = 4, refundable = true } },
            };
            // Setup the pieces localization keys
            CrystalBrazierBowl.BeforePrefabRegistered = (jbuildpiece) => {
                Piece piece = jbuildpiece.Objs.Prefab.GetComponent<Piece>();
                piece.m_name = $"$EJ_Brazier_{name}";
                piece.m_description = $"$EJ_Brazier_{name}_Description";
                                // Set color of the gem light and flare to match the gem color
                jbuildpiece.Objs.Prefab.transform.GetComponentInChildren<Light>().color = GemDefinitions[name].Color;
                ParticleSystem.MainModule particleMain = jbuildpiece.Objs.Prefab.transform.GetComponentInChildren<ParticleSystem>().main;
                particleMain.startColor = GemDefinitions[name].Color;
            };
            JotunnPiece.RegisterJotunnPiece(CrystalBrazierBowl);

            // Chandelier
            JotunnPiece.JotunnBuildPiece Chandelier = new JotunnPiece.JotunnBuildPiece();
            Chandelier.Name = $"{name} Chandelier";
            Chandelier.Prefab = $"CL_Chandelier_{name}";
            Chandelier.Sprite = $"CL_Chandelier_{name}";
            Chandelier.Workbench = "forge";
            Chandelier.Category = "Crystal Lights";
            Chandelier.PieceCost = new List<JotunnPiece.PieceCost>() {
                { new JotunnPiece.PieceCost() { prefab = $"Uncut_{name.ToLower()}_Stone", amount = 15, refundable = true } },
                { new JotunnPiece.PieceCost() { prefab = "IronNails", amount = 5, refundable = true } },
            };
            // Setup the pieces localization keys
            Chandelier.BeforePrefabRegistered = (jbuildpiece) => {
                Piece piece = jbuildpiece.Objs.Prefab.GetComponent<Piece>();
                piece.m_name = $"$EJ_Chandelier_{name}";
                piece.m_description = $"$EJ_Chandelier_{name}_Description";
                // Set color of the gem light and flare to match the gem color
                jbuildpiece.Objs.Prefab.transform.GetComponentInChildren<Light>().color = GemDefinitions[name].Color;
                ParticleSystem.MainModule particleMain = jbuildpiece.Objs.Prefab.transform.GetComponentInChildren<ParticleSystem>().main;
                particleMain.startColor = GemDefinitions[name].Color;
            };
            JotunnPiece.RegisterJotunnPiece(Chandelier);

            // Hanging bowl
            JotunnPiece.JotunnBuildPiece HangingBowl = new JotunnPiece.JotunnBuildPiece();
            HangingBowl.Name = $"{name} Hanging Bowl";
            HangingBowl.Prefab = $"CL_Hanging_{name}";
            HangingBowl.Sprite = $"CL_Hanging_{name}";
            HangingBowl.Workbench = "forge";
            HangingBowl.Category = "Crystal Lights";
            HangingBowl.PieceCost = new List<JotunnPiece.PieceCost>() {
                { new JotunnPiece.PieceCost() { prefab = $"Uncut_{name.ToLower()}_Stone", amount = 5, refundable = true } },
                { new JotunnPiece.PieceCost() { prefab = "IronNails", amount = 2, refundable = true } },
            };
            // Setup the pieces localization keys
            HangingBowl.BeforePrefabRegistered = (jbuildpiece) => {
                Piece piece = jbuildpiece.Objs.Prefab.GetComponent<Piece>();
                piece.m_name = $"$EJ_Hanging_{name}";
                piece.m_description = $"$EJ_Hanging_{name}_Description";
                // Set color of the gem light and flare to match the gem color
                jbuildpiece.Objs.Prefab.transform.GetComponentInChildren<Light>().color = GemDefinitions[name].Color;
                ParticleSystem.MainModule particleMain = jbuildpiece.Objs.Prefab.transform.GetComponentInChildren<ParticleSystem>().main;
                particleMain.startColor = GemDefinitions[name].Color;
            };
            JotunnPiece.RegisterJotunnPiece(HangingBowl);

            // Large wall light
            JotunnPiece.JotunnBuildPiece LargeWallLight = new JotunnPiece.JotunnBuildPiece();
            LargeWallLight.Name = $"{name} Large Wall Light";
            LargeWallLight.Prefab = $"CL_Large_Wall_{name}";
            LargeWallLight.Sprite = $"CL_Large_Wall_{name}";
            LargeWallLight.Workbench = "forge";
            LargeWallLight.Category = "Crystal Lights";
            LargeWallLight.PieceCost = new List<JotunnPiece.PieceCost>() {
                { new JotunnPiece.PieceCost() { prefab = $"Uncut_{name.ToLower()}_Stone", amount = 10, refundable = true } },
                { new JotunnPiece.PieceCost() { prefab = "IronNails", amount = 2, refundable = true } },
            };
            // Setup the pieces localization keys
            LargeWallLight.BeforePrefabRegistered = (jbuildpiece) => {
                Piece piece = jbuildpiece.Objs.Prefab.GetComponent<Piece>();
                piece.m_name = $"$EJ_Large_Wall_{name}";
                piece.m_description = $"$EJ_Large_Wall_{name}_Description";
                // Set color of the gem light and flare to match the gem color
                jbuildpiece.Objs.Prefab.transform.GetComponentInChildren<Light>().color = GemDefinitions[name].Color;
                ParticleSystem.MainModule particleMain = jbuildpiece.Objs.Prefab.transform.GetComponentInChildren<ParticleSystem>().main;
                particleMain.startColor = GemDefinitions[name].Color;
            };
            JotunnPiece.RegisterJotunnPiece(LargeWallLight);

            // Pole light
            JotunnPiece.JotunnBuildPiece PoleLight = new JotunnPiece.JotunnBuildPiece();
            PoleLight.Name = $"{name} Pole Light";
            PoleLight.Prefab = $"CL_Pole_{name}";
            PoleLight.Sprite = $"CL_Pole_{name}";
            PoleLight.Workbench = "forge";
            PoleLight.Category = "Crystal Lights";
            PoleLight.PieceCost = new List<JotunnPiece.PieceCost>() {
                { new JotunnPiece.PieceCost() { prefab = $"Uncut_{name.ToLower()}_Stone", amount = 5, refundable = true } },
                { new JotunnPiece.PieceCost() { prefab = "Iron", amount = 2, refundable = true } },
            };
            // Setup the pieces localization keys
            PoleLight.BeforePrefabRegistered = (jbuildpiece) => {
                Piece piece = jbuildpiece.Objs.Prefab.GetComponent<Piece>();
                piece.m_name = $"$EJ_Pole_{name}";
                piece.m_description = $"$EJ_Pole_{name}_Description";
                // Set color of the gem light and flare to match the gem color
                jbuildpiece.Objs.Prefab.transform.GetComponentInChildren<Light>().color = GemDefinitions[name].Color;
                ParticleSystem.MainModule particleMain = jbuildpiece.Objs.Prefab.transform.GetComponentInChildren<ParticleSystem>().main;
                particleMain.startColor = GemDefinitions[name].Color;
            };
            JotunnPiece.RegisterJotunnPiece(PoleLight);

            // Raw Crystal Light
            JotunnPiece.JotunnBuildPiece RawCrystal = new JotunnPiece.JotunnBuildPiece();
            RawCrystal.Name = $"{name} Raw Crystal";
            RawCrystal.Prefab = $"CL_Raw_{name}";
            RawCrystal.Sprite = $"CL_Raw_{name}";
            RawCrystal.Workbench = "piece_workbench";
            RawCrystal.Category = "Crystal Lights";
            RawCrystal.PieceCost = new List<JotunnPiece.PieceCost>() {
                { new JotunnPiece.PieceCost() { prefab = $"Uncut_{name.ToLower()}_Stone", amount = 5, refundable = true } },
            };
            // Setup the pieces localization keys
            RawCrystal.BeforePrefabRegistered = (jbuildpiece) => {
                Piece piece = jbuildpiece.Objs.Prefab.GetComponent<Piece>();
                piece.m_name = $"$EJ_Raw_{name}";
                piece.m_description = $"$EJ_Raw_{name}_Description";
                // Set color of the gem light and flare to match the gem color
                jbuildpiece.Objs.Prefab.transform.GetComponentInChildren<Light>().color = GemDefinitions[name].Color;
                ParticleSystem.MainModule particleMain = jbuildpiece.Objs.Prefab.transform.GetComponentInChildren<ParticleSystem>().main;
                particleMain.startColor = GemDefinitions[name].Color;
            };
            JotunnPiece.RegisterJotunnPiece(RawCrystal);

            // Small Wall Light
            JotunnPiece.JotunnBuildPiece SmallWallLight = new JotunnPiece.JotunnBuildPiece();
            SmallWallLight.Name = $"{name} Small Wall Light";
            SmallWallLight.Prefab = $"CL_Small_Wall_{name}";
            SmallWallLight.Sprite = $"CL_Small_Wall_{name}";
            SmallWallLight.Workbench = "forge";
            SmallWallLight.Category = "Crystal Lights";
            SmallWallLight.PieceCost = new List<JotunnPiece.PieceCost>() {
                { new JotunnPiece.PieceCost() { prefab = $"Uncut_{name.ToLower()}_Stone", amount = 3, refundable = true } },
                { new JotunnPiece.PieceCost() { prefab = "IronNails", amount = 1, refundable = true } },
            };
            // Setup the pieces localization keys
            SmallWallLight.BeforePrefabRegistered = (jbuildpiece) => {
                Piece piece = jbuildpiece.Objs.Prefab.GetComponent<Piece>();
                piece.m_name = $"$EJ_Small_Wall_{name}";
                piece.m_description = $"$EJ_Small_Wall_{name}_Description";
                // Set color of the gem light and flare to match the gem color
                jbuildpiece.Objs.Prefab.transform.GetComponentInChildren<Light>().color = GemDefinitions[name].Color;
                ParticleSystem.MainModule particleMain = jbuildpiece.Objs.Prefab.transform.GetComponentInChildren<ParticleSystem>().main;
                particleMain.startColor = GemDefinitions[name].Color;
            };
            JotunnPiece.RegisterJotunnPiece(SmallWallLight);

            // Standing Lamp
            JotunnPiece.JotunnBuildPiece StandingLamp = new JotunnPiece.JotunnBuildPiece();
            StandingLamp.Name = $"{name} Standing Lamp";
            StandingLamp.Prefab = $"CL_Standing_Lamp_{name}";
            StandingLamp.Sprite = $"CL_Standing_Lamp_{name}";
            StandingLamp.Workbench = "forge";
            StandingLamp.Category = "Crystal Lights";
            StandingLamp.PieceCost = new List<JotunnPiece.PieceCost>() {
                { new JotunnPiece.PieceCost() { prefab = $"Uncut_{name.ToLower()}_Stone", amount = 1, refundable = true } },
                { new JotunnPiece.PieceCost() { prefab = "IronNails", amount = 1, refundable = true } },
            };
            // Setup the pieces localization keys
            StandingLamp.BeforePrefabRegistered = (jbuildpiece) => {
                Piece piece = jbuildpiece.Objs.Prefab.GetComponent<Piece>();
                piece.m_name = $"$EJ_Standing_Lamp_{name}";
                piece.m_description = $"$EJ_Standing_Lamp_{name}_Description";
                // Set color of the gem light and flare to match the gem color
                jbuildpiece.Objs.Prefab.transform.GetComponentInChildren<Light>().color = GemDefinitions[name].Color;
                ParticleSystem.MainModule particleMain = jbuildpiece.Objs.Prefab.transform.GetComponentInChildren<ParticleSystem>().main;
                particleMain.startColor = GemDefinitions[name].Color;
            };
            JotunnPiece.RegisterJotunnPiece(StandingLamp);

            // Standing Torch
            JotunnPiece.JotunnBuildPiece StandingTorch = new JotunnPiece.JotunnBuildPiece();
            StandingTorch.Name = $"{name} Standing Torch";
            StandingTorch.Prefab = $"CL_Standing_Torch_{name}";
            StandingTorch.Sprite = $"CL_Standing_Torch_{name}";
            StandingTorch.Workbench = "none";
            StandingTorch.Category = "Crystal Lights";
            StandingTorch.PieceCost = new List<JotunnPiece.PieceCost>() {
                { new JotunnPiece.PieceCost() { prefab = $"Uncut_{name.ToLower()}_Stone", amount = 1, refundable = true } },
                { new JotunnPiece.PieceCost() { prefab = "Wood", amount = 2, refundable = true } },
            };
            // Setup the pieces localization keys
            StandingTorch.BeforePrefabRegistered = (jbuildpiece) => {
                Piece piece = jbuildpiece.Objs.Prefab.GetComponent<Piece>();
                piece.m_name = $"$EJ_Standing_Torch_{name}";
                piece.m_description = $"$EJ_Standing_Torch_{name}_Description";
                // Set color of the gem light and flare to match the gem color
                jbuildpiece.Objs.Prefab.transform.GetComponentInChildren<Light>().color = GemDefinitions[name].Color;
                ParticleSystem.MainModule particleMain = jbuildpiece.Objs.Prefab.transform.GetComponentInChildren<ParticleSystem>().main;
                particleMain.startColor = GemDefinitions[name].Color;
            };
            JotunnPiece.RegisterJotunnPiece(StandingTorch);
        }
    }
}

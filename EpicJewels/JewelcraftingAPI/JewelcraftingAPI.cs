// Decompiled with JetBrains decompiler
// Type: Jewelcrafting.API
// Assembly: Jewelcrafting, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 98394ACF-5603-4F15-935A-701EA584C831
// Assembly location: C:\Users\carls\Documents\projects\Valheim_Stuff\Jewelcrafting_Epic_Jewels\EpicJewels\JewelcraftingAPI.dll

using BepInEx.Configuration;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;


#nullable enable
namespace Jewelcrafting {
    [Flags]
    public enum VisualEffectCondition : uint {
        IsSkill = 4095, // 0x00000FFF
        Swords = 1,
        Knives = 2,
        Clubs = Knives | Swords, // 0x00000003
        Polearms = 4,
        Spears = Polearms | Swords, // 0x00000005
        Blocking = Polearms | Knives, // 0x00000006
        Axes = Blocking | Swords, // 0x00000007
        Bows = 8,
        Unarmed = Bows | Clubs, // 0x0000000B
        Pickaxes = Bows | Polearms, // 0x0000000C
        WoodCutting = Pickaxes | Swords, // 0x0000000D
        Crossbows = Pickaxes | Knives, // 0x0000000E
        IsItem = 1044480, // 0x000FF000
        Helmet = 24576, // 0x00006000
        Chest = 28672, // 0x00007000
        Legs = 45056, // 0x0000B000
        Hands = 49152, // 0x0000C000
        Shoulder = 69632, // 0x00011000
        Tool = 77824, // 0x00013000
        GenericExtraAttributes = 4278190080, // 0xFF000000
        Blackmetal = 1073741824, // 0x40000000
        TwoHanded = 2147483648, // 0x80000000
        SpecificExtraAttributes = 15728640, // 0x00F00000
        Hammer = 1126400, // 0x00113000
        Hoe = 2174976, // 0x00213000
        Buckler = 1048582, // 0x00100006
        Towershield = 2097158, // 0x00200006
        FineWoodBow = 1048584, // 0x00100008
        BowHuntsman = 2097160, // 0x00200008
        BowDraugrFang = 3145736, // 0x00300008
        PickaxeIron = FineWoodBow | Polearms, // 0x0010000C
        Club = 1048579, // 0x00100003
    }

    [AttributeUsage(AttributeTargets.Field)]
    public abstract class PowerAttribute : Attribute {
        public abstract float Add(float a, float b);
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class OptionalPowerAttribute : Attribute {
        public readonly float DefaultValue;

        public OptionalPowerAttribute(float defaultValue) => this.DefaultValue = defaultValue;
    }

    public class MultiplicativePercentagePowerAttribute : PowerAttribute {
        public override float Add(float a, float b) {
            return (float)(((1.0 + (double)a / 100.0) * (1.0 + (double)b / 100.0) - 1.0) * 100.0);
        }
    }

    public class MinPowerAttribute : PowerAttribute {
        public override float Add(float a, float b) => Mathf.Min(a, b);
    }

    public class MaxPowerAttribute : PowerAttribute {
        public override float Add(float a, float b) => Mathf.Max(a, b);
    }

    public class InverseMultiplicativePercentagePowerAttribute : PowerAttribute {
        public override float Add(float a, float b) {
            return (float)((1.0 - (1.0 - (double)a / 100.0) * (1.0 - (double)b / 100.0)) * 100.0);
        }
    }

    public class AdditivePowerAttribute : PowerAttribute {
        public override float Add(float a, float b) => a + b;
    }

    [PublicAPI]
    public static class API {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
        public static event Action? OnEffectRecalc;

        public static bool IsLoaded() => false;

        internal static void InvokeEffectRecalc() {

            Action onEffectRecalc = API.OnEffectRecalc;

            if (onEffectRecalc == null)
                return;
            onEffectRecalc();
        }

        public static GameObject CreateNecklaceFromTemplate(string colorName, Color color) {

            return (GameObject)null;
        }

        public static GameObject CreateNecklaceFromTemplate(string colorName, Material material) {
            return (GameObject)null;
        }

        public static GameObject CreateRingFromTemplate(string colorName, Color color) {
            return (GameObject)null;
        }

        public static GameObject CreateRingFromTemplate(string colorName, Material material) {
            return (GameObject)null;
        }

        public static void MarkJewelry(GameObject jewelry) {
        }

        public static void AddGems(string type, string colorName, Color color) {
        }

        public static List<GameObject> AddGems(
          string type,
          string colorName,
          Material material,
          Color color) {
            return (List<GameObject>)null;
        }

        public static GameObject AddDestructibleFromTemplate(
          string type,
          string colorName,
          Color color) {
            return (GameObject)null;
        }

        public static GameObject AddDestructibleFromTemplate(
          string type,
          string colorName,
          Material material) {
            return (GameObject)null;
        }

        public static GameObject AddUncutFromTemplate(string type, string colorName, Color color) {
            return (GameObject)null;
        }

        public static GameObject AddUncutFromTemplate(string type, string colorName, Material material) {
            return (GameObject)null;
        }

        public static GameObject AddAndRegisterUncutFromTemplate(
          string type,
          string colorName,
          Color color) {
            return (GameObject)null;
        }

        public static GameObject AddAndRegisterUncutFromTemplate(
          string type,
          string colorName,
          Material material) {
            return (GameObject)null;
        }

        public static GameObject AddShardFromTemplate(string type, string colorName, Color color) {
            return (GameObject)null;
        }

        public static GameObject AddShardFromTemplate(string type, string colorName, Material material) {
            return (GameObject)null;
        }

        public static GameObject[] AddTieredGemFromTemplate(string type, string colorName, Color color) {
            return (GameObject[])null;
        }

        public static GameObject[] AddTieredGemFromTemplate(
          string type,
          string colorName,
          Material material,
          Color color) {
            return (GameObject[])null;
        }

        public static void AddGem(GameObject prefab, string colorName) {
        }

        public static void AddShard(GameObject prefab, string colorName) {
        }

        public static void AddDestructible(GameObject prefab, string colorName) {
        }

        public static void AddUncutGem(
          GameObject prefab,
          string colorName,
          ConfigEntry<float>? dropChance = null) {
        }

        public static void AddGemEffect<T>(
          string name,
          string? englishDescription = null,
          string? englishDescriptionDetailed = null)
          where T : struct {
        }

        public static void AddGemConfig(string yaml) {
        }

        public static T GetEffectPower<T>(this Player player, string name) where T : struct {
            return default(T);
        }

#pragma warning disable IDE0090 // Use 'new(...)'
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        public static List<API.GemInfo?> GetGems(ItemDrop.ItemData item) => new List<API.GemInfo>();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
#pragma warning restore IDE0090 // Use 'new(...)'

        public static bool SetGems(ItemDrop.ItemData item, List<API.GemInfo?> gems) => false;

        public static Sprite GetSocketBorder() => (Sprite)null;

        public static GameObject GetGemcuttersTable() => (GameObject)null;

        public static void AddParticleEffect(
          string prefabName,
          GameObject effect,
          VisualEffectCondition displayCondition) {
        }

        public static void SetSocketsLock(ItemDrop.ItemData item, bool enabled) {
        }

        public static void OnGemBreak(API.GemBreakHandler callback) {
        }

        public static void OnItemBreak(API.ItemBreakHandler callback) {
        }

        public static void OnItemMirrored(API.ItemMirroredHandler callback) {
        }

        public static bool IsJewelryEquipped(Player player, string prefabName) => false;

        public static bool BlacklistItem(GameObject item) => false;

        [PublicAPI]
        public class GemInfo {
            public readonly string gemPrefab;
            public readonly Sprite gemSprite;
            public readonly Dictionary<string, float> gemEffects;

            public GemInfo(string gemPrefab, Sprite gemSprite, Dictionary<string, float> gemEffects) {
                this.gemPrefab = gemPrefab;
                this.gemSprite = gemSprite;
                this.gemEffects = gemEffects;
            }
        }

        public delegate bool GemBreakHandler(
          ItemDrop.ItemData? container,
          ItemDrop.ItemData gem,
          int count = 1);

        public delegate bool ItemBreakHandler(ItemDrop.ItemData? container);

        public delegate bool ItemMirroredHandler(ItemDrop.ItemData? item);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
    }

}

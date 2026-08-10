using HarmonyLib;
using JetBrains.Annotations;
using Jewelcrafting;
using System;
using System.Collections.Generic;
using UnityEngine;
using Logger = EpicJewels.Common.EJLogger;

namespace EpicJewels.GemEffects
{
    public static class Farmer
    {
        [PublicAPI]
        public struct Config
        {
            [AdditivePowerAttribute] public float Power;
            [AdditivePowerAttribute] public float Chance;
            [AdditivePowerAttribute] public float Pickup;
        }

        static List<String> UnallowedGreenThumbPickables = new List<String>() { 
            "SurtlingCore", 
            "Flint", 
            "Wood",
            "Stone",
            "Amber",
            "AmberPearl",
            "Coins",
            "Ruby",
            "CryptRemains",
            "Obsidian",
            "Crystal",
            "Pot_Shard",
            "DragonEgg",
            "DvergrLantern",
            "DvergrMineTreasure",
            "SulfurRock",
            "VoltureEgg",
            "Swordpiece",
            "MoltenCore",
            "Hairstrands",
            "Tar",
            "BlackCore"
        };

        // Both the manual-pick and auto-pickup paths have to derive the blocklist key the same way,
        // otherwise the two filter different sets. Keyed off the dropped item prefab, not the
        // Pickable GameObject, since that is what the blocklist entries name.
        private static bool IsBlockedPickable(Pickable pickable) {
            if (pickable == null || pickable.m_itemPrefab == null) { return true; }
            string prefabname = pickable.m_itemPrefab.name.Replace("(Clone)", "").Replace("Pickable_", "");
            return UnallowedGreenThumbPickables.Contains(prefabname);
        }

        [HarmonyPatch(typeof(Pickable), nameof(Pickable.Interact))]
        public static class IncreaseCarryWeight {
            // Capture whether the pickable was already picked BEFORE Interact runs.
            // Interact routes RPC_Pick to the ZDO owner; it only resolves synchronously
            // (flipping m_picked) when the LOCAL player owns the ZDO. For a non-owner the
            // pick is async, so without this guard the bonus below duplicates on every call
            // (e.g. via the 0.5s auto-pickup loop) while the bush still reads as pickable.
            public static void Prefix(Pickable __instance, out bool __state) {
                __state = __instance.GetPicked();
            }

            public static void Postfix(bool __result, Humanoid character, Pickable __instance, bool __state) {
                // Only reward a genuine unpicked -> picked transition caused by THIS interaction.
                // __result == false : pickable had no interact animation (unchanged from original behaviour).
                // __state           : it was already picked before this call, so no reward.
                // !GetPicked()       : the pick did not resolve locally (we are not the ZDO owner) -> no reward.
                if (__result == false || __state || __instance.GetPicked() == false) {
                    // No local picking happened
                    return;
                }
                // Being picked by the current player
                if (character != null && character is Player player && player.GetEffectPower<Config>("Farmer").Power > 0) {
                    if (IsBlockedPickable(__instance)) {
                        // EpicJewels.EJLog.LogDebug($"Pickable type is not allowed for farmer perk.");
                        return;
                    }
                    float roll = UnityEngine.Random.value;
                    float chance_max = (player.GetEffectPower<Config>("Farmer").Chance / 100);
                    // EpicJewels.EJLog.LogDebug($"Farmer chance roll: {roll} < {chance_max}");
                    if (roll < chance_max) {
                        int offset = 0;
                        for (int i = 0; i < player.GetEffectPower<Config>("Farmer").Power; i++) {
                            __instance.Drop(__instance.m_itemPrefab, offset++, 1);
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.Update))]
        public static class AutoPickupNearby_Pickables
        {
            private static readonly int pickableMask = LayerMask.GetMask("piece_nonsolid", "item", "Default_small");

            private static float last_update = 0f;
            public static void Postfix(Player __instance)
            {
                if (__instance != null && __instance.GetEffectPower<Config>("Farmer").Pickup > 0) {
                    // We only want to do this silly expensive update every half a second or so.
                    // Driven off Time.time rather than an accumulator: this runs in Update, so a
                    // fixedDeltaTime accumulator throttled by frame count instead of wall time and
                    // lost precision over a long session.
                    if (Time.time < (last_update + 0.5f)) { return; }
                    last_update = Time.time;
                    foreach (Collider obj_collider in Physics.OverlapSphere(__instance.transform.position, (2f + __instance.GetEffectPower<Config>("Farmer").Pickup), pickableMask)) {
                        Pickable pickable_item = obj_collider.GetComponent<Pickable>() ?? obj_collider.GetComponentInParent<Pickable>();
                        if (pickable_item != null && IsBlockedPickable(pickable_item) == false) {
                            if (pickable_item.m_nview != null && pickable_item.CanBePicked()) {
                                // Logger.LogDebug($"Autopicking: {pickable_item.m_itemPrefab.name}");
                                pickable_item.m_nview.ClaimOwnership();
                                pickable_item.Interact(__instance, false, false);
                            }
                        }
                    }
                }
            }

        }
    }
}

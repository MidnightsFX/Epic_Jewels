using BepInEx.Logging;
using EpicJewels.Common;
using HarmonyLib;
using JetBrains.Annotations;
using Jewelcrafting;

namespace EpicJewels.GemEffects {
    internal class EitrFeedback {
        [PublicAPI]
        public struct Config {
            [InverseMultiplicativePercentagePower] public float Power;
            [InverseMultiplicativePercentagePower] public float Chance;
        }

        [HarmonyPatch(typeof(Player), nameof(Player.RPC_UseEitr))]
        public static class ReducePierceDamageTaken {
            [UsedImplicitly]
            private static void Prefix(Player __instance, float v) {
                if (__instance.m_eitr <= 0 || v < 1) { return; } // Skip eitr-less targets trying to use eitr or no consume eitr calls
                float chance = __instance.GetEffectPower<Config>("Eitr Feedback").Chance;
                if (chance > 0 && UnityEngine.Random.Range(0f, 100f) <= chance) {
                    float power = __instance.GetEffectPower<Config>("Eitr Feedback").Power;
                    EJLogger.LogDebug($"Eitr Feedback restoring {power} adrenaline from eitr use.");
                    __instance.AddAdrenaline(power);
                }
            }
        }
    }
}

using EpicJewels.Common;
using HarmonyLib;
using JetBrains.Annotations;
using Jewelcrafting;

namespace EpicJewels.GemEffects {
    internal class SoakedFury {
        [PublicAPI]
        public struct Config {
            [MultiplicativePercentagePowerAttribute] public float Power;
        }

        [HarmonyPatch(typeof(Player), nameof(Player.AddAdrenaline))]
        public static class ReducePoisonDamageTaken {

            private static readonly int Wetstatus = "Wet".GetStableHashCode();

            [UsedImplicitly]
            private static void Prefix(Player __instance, ref float v) {
                float wetfury = __instance.GetEffectPower<Config>("Soaked Fury").Power;
                if (wetfury > 0 && __instance.GetSEMan().HaveStatusEffect(Wetstatus)) {
                    float multi = (wetfury + 100) / 100f;
                    EJLogger.LogDebug($"Soaked Fury is increasing adrenaline gain by {multi}");
                    v *= multi;
                }
            }
        }
    }
}

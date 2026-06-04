using EpicJewels.Common;
using HarmonyLib;
using JetBrains.Annotations;
using Jewelcrafting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicJewels.GemEffects {
    internal class BurningAdrenaline {
        [PublicAPI]
        public struct Config {
            [MultiplicativePercentagePowerAttribute] public float Power;
        }

        [HarmonyPatch(typeof(Player), nameof(Player.AddAdrenaline))]
        public static class ReducePoisonDamageTaken {

            private static readonly int Wetstatus = "Burning".GetStableHashCode();

            [UsedImplicitly]
            private static void Prefix(Player __instance, ref float v) {
                float wetfury = __instance.GetEffectPower<Config>("Burning Adrenaline").Power;
                if (wetfury > 0 && __instance.GetSEMan().HaveStatusEffect(Wetstatus)) {
                    float multi = (wetfury + 100) / 100f;
                    EJLogger.LogDebug($"Burning Adrenaline is increasing adrenaline gain by {multi}");
                    v *= multi;
                }
            }
        }
    }
}

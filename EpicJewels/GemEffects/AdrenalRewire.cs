using HarmonyLib;
using JetBrains.Annotations;
using Jewelcrafting;
using EpicJewels.EffectHelpers;
using EpicJewels.Common;

namespace EpicJewels.GemEffects {
    internal class AdrenalRewire {
        [PublicAPI]
        public struct Config {
            [InverseMultiplicativePercentagePower] public float Power;
            [InverseMultiplicativePercentagePower] public float Chance;
        }

        [HarmonyPatch(typeof(Character), nameof(Character.RPC_Damage))]
        public static class DamageTakenProvidesStamina {
            public static void Postfix(Character __instance, HitData hit) {
                if (__instance.IsPlayer() == false) { return; }
                Player player = __instance as Player;

                float chance = player.GetEffectPower<Config>("Adrenal Rewire").Chance;
                if (UnityEngine.Random.Range(0f, 100f) > chance) { return; }

                float power = player.GetEffectPower<Config>("Adrenal Rewire").Power;
                if (power > 0) {
                    float totaldmg = hit.m_damage.GetTotalDamageOptions();
                    float adrenaline_from_dmg = totaldmg * (power / (100f + __instance.GetMaxAdrenaline()));
                    EJLogger.LogDebug($"Adrenal Rewire is restoring {adrenaline_from_dmg} stamina based on total damage of {totaldmg} and power of {power}");
                    __instance.AddAdrenaline(adrenaline_from_dmg);
                }
            }
        }
    }
}

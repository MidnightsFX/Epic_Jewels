using HarmonyLib;
using JetBrains.Annotations;
using Jewelcrafting;
using UnityEngine;


namespace EpicJewels.GemEffects {
    internal class Toxifier {
        [PublicAPI]
        public struct Config {
            [MultiplicativePercentagePowerAttribute] public float Power;
            [MultiplicativePercentagePowerAttribute] public float Distance;
        }

        [HarmonyPatch(typeof(Character), nameof(Character.AddPoisonDamage))]
        public static class ReducePoisonDamageTaken {
            [UsedImplicitly]
            private static void Postfix(Character __instance) {
                if (Player.m_localPlayer != null && __instance != Player.m_localPlayer) {
                    float toxipower = Player.m_localPlayer.GetEffectPower<Config>("Toxifier").Power;
                    if (toxipower > 0 && Player.m_localPlayer.GetEffectPower<Config>("Toxifier").Distance > Vector3.Distance(Player.m_localPlayer.transform.position, __instance.transform.position)) {
                        Player.m_localPlayer.AddAdrenaline(toxipower);
                    }
                }
            }
        }
    }
}

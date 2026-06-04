using EpicJewels.Common;
using EpicJewels.EffectHelpers;
using HarmonyLib;
using JetBrains.Annotations;
using Jewelcrafting;

namespace EpicJewels.GemEffects {
    internal class HarvestAdrenaline {
        [PublicAPI]
        public struct Config {
            [InverseMultiplicativePercentagePower] public float Power;
            [InverseMultiplicativePercentagePower] public float Chance;
        }

        public static void AddadrenalineForHarvest(HitData hit) {
            if ((hit.m_damage.m_chop > 0 || hit.m_damage.m_pickaxe > 0) && hit.GetAttacker() is Player) {
                float randomValue = UnityEngine.Random.Range(0f, 100f);
                Player attacker = hit.GetAttacker() as Player;
                // Succeeds chance roll
                if (randomValue <= attacker.GetEffectPower<Config>("Harvest Adrenaline").Chance) {
                    float totaldmg = hit.m_damage.GetTotalDamageOptions(include_pickaxe_and_chop: true);
                    float power = attacker.GetEffectPower<Config>("Harvest Adrenaline").Power;
                    float amount = totaldmg * (power / 100f);
                    EJLogger.LogDebug($"Harvest Adrenaline is restoring {amount} adrenaline based on total damage of {totaldmg} and power of {power}");
                    attacker.AddAdrenaline(amount);
                }
            }
        }

        [HarmonyPatch(typeof(Destructible), nameof(Destructible.RPC_Damage))]
        public static class AdrenalineFromDestructibles {
            public static void Postfix(HitData hit) { AddadrenalineForHarvest(hit); }
        }

        [HarmonyPatch(typeof(MineRock), nameof(MineRock.RPC_Hit))]
        public static class AdrenalineFromMineRocks {
            public static void Postfix(HitData hit) {
                AddadrenalineForHarvest(hit);
            }
        }

        [HarmonyPatch(typeof(MineRock5), nameof(MineRock5.RPC_Damage))]
        public static class AdrenalineFromMineRocks5 {
            public static void Postfix(HitData hit) {
                AddadrenalineForHarvest(hit);
            }
        }

        [HarmonyPatch(typeof(TreeBase), nameof(TreeBase.RPC_Damage))]
        public static class AdrenalineFromTreeBase {
            public static void Postfix(HitData hit) {
                AddadrenalineForHarvest(hit);
            }
        }

        [HarmonyPatch(typeof(TreeLog), nameof(TreeLog.RPC_Damage))]
        public static class AdrenalineFromTreeLog {
            public static void Postfix(HitData hit) {
                AddadrenalineForHarvest(hit);
            }
        }
    }
}

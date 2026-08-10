using EpicJewels.EffectHelpers;
using HarmonyLib;
using JetBrains.Annotations;
using Jewelcrafting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Logger = EpicJewels.Common.EJLogger;

namespace EpicJewels.GemEffects
{
    public static class BlockReduceStamina
    {
        [PublicAPI]
        public struct Config
        {
            [AdditivePowerAttribute] public float Power;
        }

        [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.BlockAttack))]
        public static class ModifyBlockStaminaUse_Humanoid_BlockAttack_Patch
        {
            // m_blockStaminaDrain is a persistent field on the Humanoid, so the reduction has to be
            // undone in the Postfix. Multiplying it in place without restoring compounds on every
            // block and drives the drain to zero over a play session.
            public static void Prefix(Humanoid __instance, out float __state)
            {
                __state = __instance.m_blockStaminaDrain;
                if (__instance is Player player) {
                    if (player.GetEffectPower<Config>("Block Reduce Stamina").Power > 0) {
                        float block_stamina_multiplier = 100f / (100f + player.GetEffectPower<Config>("Block Reduce Stamina").Power);
                        // Logger.LogDebug($"Multiplying block stamina cost by {block_stamina_multiplier}");
                        __instance.m_blockStaminaDrain *= block_stamina_multiplier;
                    }

                }
            }

            public static void Postfix(Humanoid __instance, float __state)
            {
                __instance.m_blockStaminaDrain = __state;
            }
        }
    }
}

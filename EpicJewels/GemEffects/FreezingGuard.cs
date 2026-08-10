using HarmonyLib;
using JetBrains.Annotations;
using Jewelcrafting;
using Logger = EpicJewels.Common.EJLogger;

namespace EpicJewels.GemEffects
{
    internal class FreezingGuard
    {
        [PublicAPI]
        public struct Config
        {
            [AdditivePowerAttribute] public float Power;
            [AdditivePowerAttribute] public float Chance;
        }

        [HarmonyPriority(Priority.High)]
        [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.BlockAttack))]
        private static class FreezingBlock_Patch
        {
            // Carried through __state rather than a static: the Postfix calls attacker.Damage(),
            // which can re-enter BlockAttack if the attacker is itself blocking and would clobber a
            // shared field before the outer Postfix reads it.
            private static void Prefix(HitData hit, out float __state)
            {
                __state = hit.GetTotalDamage();
            }

            private static int froststatus = "Frost".GetStableHashCode();
            private static void Postfix(Humanoid __instance, HitData hit, Character attacker, ref bool __result, float __state)
            {
                if (__instance is Player player && player.GetEffectPower<Config>("Freezing Guard").Chance > 0 && __result == true && attacker != null && attacker.IsDead() == false)
                {
                    float roll = UnityEngine.Random.value;
                    float chance_max = (player.GetEffectPower<Config>("Freezing Guard").Chance / 100);
                    // Logger.LogDebug($"Freezing guard chance roll: {roll} < {chance_max}");
                    if (roll < chance_max)
                    {
                        HitData frost_rebuke_hit = new HitData();
                        frost_rebuke_hit.m_damage.m_frost = (player.GetEffectPower<Config>("Freezing Guard").Power / 100) * __state;
                        // Logger.LogDebug($"Hit dmg {originalHit} FreezingGuard returning damage {frost_rebuke_hit.m_damage.m_frost}");
                        frost_rebuke_hit.m_attacker = player.GetZDOID();
                        frost_rebuke_hit.m_point = hit.m_point + new UnityEngine.Vector3(0, 0.5f);
                        attacker.Damage(frost_rebuke_hit);
                        attacker.m_seman.AddStatusEffect(froststatus, true, 3, player.m_skills.GetSkill(Skills.SkillType.Blocking).m_level);
                    }
                }
            }
        }
    }
}

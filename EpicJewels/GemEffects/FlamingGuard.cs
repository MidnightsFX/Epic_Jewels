using HarmonyLib;
using JetBrains.Annotations;
using Jewelcrafting;
using Logger = EpicJewels.Common.EJLogger;

namespace EpicJewels.GemEffects
{
    public static class FlamingGuard
    {
        [PublicAPI]
        public struct Config
        {
            [AdditivePowerAttribute] public float Power;
            [AdditivePowerAttribute] public float Chance;
        }

        [HarmonyPriority(Priority.High)]
        [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.BlockAttack))]
        private static class FlamingBlock_Patch
        {
            // Carried through __state rather than a static: the Postfix calls attacker.Damage(),
            // which can re-enter BlockAttack if the attacker is itself blocking and would clobber a
            // shared field before the outer Postfix reads it.
            private static void Prefix(HitData hit, out float __state)
            {
                __state = hit.GetTotalDamage();
            }

            private static int burningstatus = "Burning".GetStableHashCode();
            private static void Postfix(Humanoid __instance, HitData hit, Character attacker, ref bool __result, float __state)
            {
                if (__instance is Player player && player.GetEffectPower<Config>("Burning Guard").Chance > 0 && __result == true && attacker != null && attacker.IsDead() == false)
                {
                    float roll = UnityEngine.Random.value;
                    float chance_max = (player.GetEffectPower<Config>("Burning Guard").Chance / 100);
                    // Logger.LogDebug($"Burning Guard chance roll: {roll} < {chance_max}");
                    if (roll < chance_max)
                    {
                        HitData flaming_rebuke_hit = new HitData();
                        flaming_rebuke_hit.m_damage.m_fire = (player.GetEffectPower<Config>("Burning Guard").Power / 100) * __state;
                        // Logger.LogDebug($"Hit dmg {originalHit} FlamingGuard returning damage {flaming_rebuke_hit.m_damage.m_fire}");
                        flaming_rebuke_hit.m_attacker = player.GetZDOID();
                        flaming_rebuke_hit.m_point = hit.m_point + new UnityEngine.Vector3(0, 0.5f);
                        attacker.Damage(flaming_rebuke_hit);
                        attacker.m_seman.AddStatusEffect(burningstatus, true, 1, player.m_skills.GetSkill(Skills.SkillType.Blocking).m_level);
                    }
                }
            }
        }
    }
}

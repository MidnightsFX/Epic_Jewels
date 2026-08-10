using HarmonyLib;
using JetBrains.Annotations;
using Jewelcrafting;
using System.Linq;
using UnityEngine;
using Logger = EpicJewels.Common.EJLogger;

namespace EpicJewels.GemEffects
{
    public static class CombatSpirit
    {
        [PublicAPI]
        public struct Config
        {
            [AdditivePowerAttribute] public float Power;
        }

        private static GameObject wolf = null;
        private static bool have_spirit_companion = false;
        private static float recheck_spirit_spawn_timer = 0;
        private static float cooldown_until = 0f;

        [HarmonyPatch(typeof(Player), nameof(Player.OnTargeted))]
        public static class CombatCompanion
        {
            private static void Postfix(Player __instance, bool sensed, bool alerted)
            {
                if (__instance.GetEffectPower<Config>("Combat Spirit").Power > 0 && sensed && alerted)
                {
                    if (wolf == null && have_spirit_companion == false)
                    {
                        ZNetScene.instance.m_namedPrefabs.TryGetValue("Wolf".GetStableHashCode(), out var temp);
                        if (temp == null) {
                            Logger.LogWarning("Combat Spirit could not resolve the 'Wolf' prefab, skipping spawn.");
                            return;
                        }

                        Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                        Vector3 player_location = __instance.gameObject.transform.position;
                        player_location.x += 1f; // spawn next to the player
                        wolf = Object.Instantiate(temp, player_location, rotation);
                        cooldown_until = Time.time + 120f;
                        have_spirit_companion = true;
                        wolf.AddComponent<CharacterTimedDestruction>();
                        Character spirit_char = wolf.GetComponent<Character>();
                        wolf.GetComponent<CharacterTimedDestruction>().m_character = spirit_char;
                        wolf.GetComponent<CharacterTimedDestruction>().Trigger(__instance.GetEffectPower<Config>("Combat Spirit").Power);
                        SkinnedMeshRenderer wolf_renderer = wolf.GetComponentInChildren<SkinnedMeshRenderer>();
                        if (wolf_renderer != null) { wolf_renderer.material = EpicJewels.spiritCreature; }
                        Object.Destroy(wolf.GetComponent<CharacterDrop>());
                        Object.Destroy(wolf.GetComponent<Tameable>());
                        Humanoid creature_metadata = wolf.GetComponent<Humanoid>();
                        Character creature_character = wolf.GetComponent<Character>();
                        MonsterAI creature_ai = wolf.GetComponent<MonsterAI>();
                        if (creature_ai != null) { creature_ai.m_attackPlayerObjects = false; }
                        // creature_character.m_level = 2;
                        creature_metadata.m_health = 1000; //lox health, but not resistant
                        for (int i = 0; i < creature_metadata.m_deathEffects.m_effectPrefabs.Length && i < 2; i++) {
                            creature_metadata.m_deathEffects.m_effectPrefabs[i].m_enabled = false;
                        }
                        creature_metadata.name = "EJ_spirit_wolf";
                        if (creature_metadata != null) {
                            creature_metadata.m_faction = Character.Faction.Players;
                        }
                        // Set this creatures lifetime
                        wolf.name = "Spirit Wolf";
                    }
                    // Logger.LogDebug($"checking for spawning spirit companion {recheck_spirit_spawn_timer} has companion? {have_spirit_companion} cooldown {cooldown_timer}");
                    // OnTargeted fires once per enemy that is tracking the player, so decrementing by
                    // deltaTime here drained the cooldown N times faster with N enemies. Compare
                    // against a wall-clock deadline instead.
                    if (Time.time < cooldown_until)
                    {
                        return;
                    }
                    // Reduce checks for spawned spirit wolf
                    if (recheck_spirit_spawn_timer > 3) {
                        recheck_spirit_spawn_timer = 0;
                        if (Character.s_characters.Any(c => Vector3.Distance(__instance.gameObject.transform.position, c.transform.position) < 100f && c.name == "Spirit Wolf")) {
                            // Logger.LogDebug("Already have spirit wolf");
                            have_spirit_companion = true;
                        } else {
                            have_spirit_companion = false;
                        }
                    } else {
                        recheck_spirit_spawn_timer += Time.deltaTime;
                    }
                }
            }
        }
    }
}

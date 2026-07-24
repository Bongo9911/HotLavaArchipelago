using HarmonyLib;
using Klei.HotLava.Game;
using System.Reflection;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Patches.Game
{
    [HarmonyPatch(typeof(ObjectPool))]
    internal class ObjectPoolPatches
    {
        /// <summary>
        /// The vanilla "RocketProjectile" pool entry was stripped from the shipped ObjectPool
        /// prefab's serialized pool list along with the rest of the rocket jump content (see
        /// PlayerRigAnimatorPatches) - but the actual prefab (real water_bomber_bottle mesh,
        /// material, and HL_RocketProjectile component) is still shipped at
        /// Resources/gameplay/RocketProjectile, just no longer registered as a pool anywhere.
        /// Re-register the real asset directly instead of using a hand-built stand-in.
        /// </summary>
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void Awake_Postfix(ObjectPool __instance)
        {
            GameObject prefab = Resources.Load<GameObject>("gameplay/RocketProjectile");

            if (prefab == null)
            {
                Plugin.Logger.LogWarning("[Archipelago] Could not find the shipped RocketProjectile prefab at Resources/gameplay/RocketProjectile; falling back to a placeholder.");
                prefab = BuildRocketProjectilePrefab();
            }
            else
            {
                FillMissingFields(prefab);
            }

            // AddObjectPool is internal to the game's assembly.
            typeof(ObjectPool)
                .GetMethod("AddObjectPool", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(__instance, new object[] { prefab, 3u });
        }

        /// <summary>
        /// The shipped prefab's RocketProjectile component may still come back with some fields
        /// unset (launch/velocity curves, collision layers, visualization target) - this asset
        /// was abandoned before it shipped as a live ability, so some of the authored data may
        /// genuinely be missing, not just an extraction artifact. Fill in anything that's still
        /// at its C# default so the projectile actually flies and collides instead of silently
        /// doing nothing.
        /// </summary>
        private static void FillMissingFields(GameObject prefab)
        {
            RocketProjectile projectile = prefab.GetComponent<RocketProjectile>();
            if (projectile == null)
            {
                return;
            }

            if (projectile.m_Visualization == null && prefab.transform.childCount > 0)
            {
                projectile.m_Visualization = prefab.transform.GetChild(0).gameObject;
            }

            if (projectile.m_LaunchCurve == null || projectile.m_LaunchCurve.length == 0)
            {
                projectile.m_LaunchCurve = AnimationCurve.Linear(0f, 0f, 1f, 10f);
            }

            if (projectile.m_VerticalVelocityMask == null || projectile.m_VerticalVelocityMask.length == 0)
            {
                projectile.m_VerticalVelocityMask = AnimationCurve.Constant(-100f, 100f, 1f);
            }

            if (projectile.m_HorizontalVelocityMask == null || projectile.m_HorizontalVelocityMask.length == 0)
            {
                projectile.m_HorizontalVelocityMask = AnimationCurve.Constant(-100f, 100f, 1f);
            }

            // Same layers LevelSingleton.m_LayerWorld is built from ("Default"/"Dynamic"). A mask
            // of 0 would mean the rocket can never hit anything, so treat that as "unset".
            int worldLayerMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Dynamic"));
            if (projectile.m_RocketCollisionLayers.value == 0)
            {
                projectile.m_RocketCollisionLayers = worldLayerMask;
            }

            if (projectile.m_ExplosionCollisionLayers.value == 0)
            {
                projectile.m_ExplosionCollisionLayers = worldLayerMask;
            }
        }

        private static GameObject BuildRocketProjectilePrefab()
        {
            GameObject root = new GameObject("RocketProjectile");
            Object.DontDestroyOnLoad(root);

            Rigidbody rigidBody = root.AddComponent<Rigidbody>();
            rigidBody.isKinematic = true;
            rigidBody.useGravity = false;

            // LaunchRocket does transform.GetChild(0).localScale = ..., so the projectile needs
            // at least one child; RocketExplode also toggles this GameObject's active state
            // directly via m_Visualization, so it has to be the same object.
            GameObject visualization = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            visualization.name = "Visualization";
            visualization.transform.SetParent(root.transform, false);
            visualization.transform.localScale = Vector3.one * 0.15f;
            Object.Destroy(visualization.GetComponent<Collider>());
            visualization.SetActive(false);

            // Same layers LevelSingleton.m_LayerWorld is built from ("Default"/"Dynamic"),
            // resolved directly via LayerMask.NameToLayer instead of reading that field: Object-
            // Pool.Awake() runs once at game boot, before any level has initialized
            // LevelSingleton, so m_LayerWorld would still read 0 (uninitialized) here and that
            // 0 would get baked into every pooled clone permanently.
            int worldLayerMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Dynamic"));

            RocketProjectile projectile = root.AddComponent<RocketProjectile>();
            projectile.m_Visualization = visualization;
            projectile.m_LaunchCurve = AnimationCurve.Linear(0f, 0f, 1f, 10f);
            projectile.m_VerticalVelocityMask = AnimationCurve.Constant(-100f, 100f, 1f);
            projectile.m_HorizontalVelocityMask = AnimationCurve.Constant(-100f, 100f, 1f);
            projectile.m_RocketCollisionLayers = worldLayerMask;
            projectile.m_ExplosionCollisionLayers = worldLayerMask;

            root.SetActive(false);
            return root;
        }
    }
}

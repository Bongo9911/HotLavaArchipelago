using HarmonyLib;
using Klei.HotLava.Game;

namespace HotLavaArchipelagoPlugin.Patches.Game
{
    [HarmonyPatch(typeof(ObjectPool))]
    internal class ObjectPoolPatches
    {
        //[HarmonyPatch("Awake")]
        //[HarmonyPostfix]
        //public static void Awake_Postfix(ObjectPool __instance)
        //{
        //    GameObject obj = new GameObject("RocketProjectile");

        //    UInt32 count = 1;

        //    typeof(ObjectPool).GetMethod("AddObjectPool", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(__instance, [obj, count]);

        //    Plugin.Logger.LogInfo("Created pool");

        //    Plugin.Logger.LogInfo(JsonConvert.SerializeObject(typeof(ObjectPool).GetField("m_PoolKeys", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance)));
        //}
    }
}

using HarmonyLib;
using Klei.HotLava.Game;
using System;
using System.Reflection;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Patches.Game
{
    [HarmonyPatch(typeof(ObjectPool))]
    internal class ObjectPoolPatches
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void Awake_Postfix(ObjectPool __instance)
        {
            GameObject obj = new GameObject("RocketProjectile");

            UInt32 count = 1;

            typeof(ObjectPool).GetMethod("AddObjectPool", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(__instance, [obj, count]);

            Plugin.Logger.LogInfo("Created pool");
        }
    }
}

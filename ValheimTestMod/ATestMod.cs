using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace ATestMod
{
    [BepInPlugin(PluginID, "ATestMod", "1.0.1")]
    public class ATestMod : BaseUnityPlugin
    {
        private static ATestMod _instance;
        private static ConfigEntry<bool> _loggingEnabled;

        public const string PluginID = "Aranethon.mods.ATestMod";

        private Harmony _harmony;

        public static void Log(string message)
        {
            if (_loggingEnabled.Value)
                _instance.Logger.LogInfo(message);
        }

        public static void LogWarning(string message)
        {
            if (_loggingEnabled.Value)
                _instance.Logger.LogWarning(message);
        }

        public static void LogError(string message)
        {
            if (_loggingEnabled.Value)
                _instance.Logger.LogError(message);
        }

        private void Awake()
        {
            _loggingEnabled = Config.Bind("Logging", "Logging Enabled", true, "Enable logging");
            _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginID);
        }

        [HarmonyPatch(typeof(InventoryGui), nameof(InventoryGui.Awake))]
        public static class InventoryGui_Awake_Patch
        {
            [HarmonyPriority(Priority.Low)]
            public static void PostFix(InventoryGui __instance)
            {
                __instance.m_containerName.color = Color.green;
            }
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
    }
}

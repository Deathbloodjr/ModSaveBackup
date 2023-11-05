﻿using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using UnityEngine;
using BepInEx.Configuration;
using ModSaveBackup.Patch;
using System.IO;

#if TAIKO_IL2CPP
using BepInEx.Unity.IL2CPP.Utils;
using BepInEx.Unity.IL2CPP;
#endif

namespace ModSaveBackup
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, ModName, PluginInfo.PLUGIN_VERSION)]
#if TAIKO_MONO
    public class Plugin : BaseUnityPlugin
#elif TAIKO_IL2CPP
    public class Plugin : BasePlugin
#endif
    {
        const string ModName = "ModSaveBackup";

        public static Plugin Instance;
        private Harmony _harmony;
        public new static ManualLogSource Log;

        public ConfigEntry<bool> ConfigEnabled;

        public ConfigEntry<bool> ConfigBackupTakoTakoSave;
        public ConfigEntry<bool> ConfigBackupSanaeSave;
        public ConfigEntry<bool> ConfigBackupDaniDojoSave;

        public ConfigEntry<string> ConfigFoldersToBackup;

        public ConfigEntry<bool> ConfigLoggingEnabled;
        public ConfigEntry<int> ConfigLoggingDetailLevelEnabled;

#if TAIKO_MONO
        private void Awake()
#elif TAIKO_IL2CPP
        public override void Load()
#endif
        {
            Instance = this;

#if TAIKO_MONO
            Log = Logger;
#elif TAIKO_IL2CPP
            Log = base.Log;
#endif

            SetupConfig();
            SetupHarmony();
        }

        private void SetupConfig()
        {
            var dataFolder = Path.Combine("BepInEx", "data", PluginInfo.PLUGIN_GUID);

            ConfigEnabled = Config.Bind("General",
                "Enabled",
                true,
                "Enables the mod.");

            ConfigBackupTakoTakoSave = Config.Bind("General",
                "BackupTakoTakoSave",
                true,
                "Automatically finds the TakoTako save folder and backs it up.");

            ConfigBackupSanaeSave = Config.Bind("General",
                "BackupSanaeSave",
                true,
                "Automatically finds the ca.sanae.saves folder and backs it up.");

            ConfigBackupDaniDojoSave = Config.Bind("General",
                "BackupDaniDojoSave",
                false,
                "Automatically finds the DaniDojo save folder and backs it up.\n" +
                "This mod has systems in place to avoid any data corruption already, so backing up may not be necessary.");

            ConfigFoldersToBackup = Config.Bind("General",
                "FoldersToBackup",
                "",
                "Backs up all set folders, with each folder separated by a '|'.\n" +
                "path/folder1|path/folder2");

            ConfigLoggingEnabled = Config.Bind("Debug",
                "LoggingEnabled",
                true,
                "Enables logs to be sent to the console.");

            ConfigLoggingDetailLevelEnabled = Config.Bind("Debug",
                "LoggingDetailLevelEnabled",
                0,
                "Enables more detailed logs to be sent to the console. The higher the number, the more logs will be displayed. Mostly for my own debugging.");
        }

        private void SetupHarmony()
        {
            // Patch methods
            _harmony = new Harmony(PluginInfo.PLUGIN_GUID);

            if (ConfigEnabled.Value)
            {

                _harmony.PatchAll(typeof(SongSelectManagerStartPatch));
                Log.LogInfo($"Plugin {PluginInfo.PLUGIN_NAME} is loaded!");
            }
            else
            {
                Log.LogInfo($"Plugin {PluginInfo.PLUGIN_NAME} is disabled.");
            }
        }

        public static MonoBehaviour GetMonoBehaviour() => TaikoSingletonMonoBehaviour<CommonObjects>.Instance;

        public void StartCustomCoroutine(IEnumerator enumerator)
        {
#if TAIKO_MONO
            GetMonoBehaviour().StartCoroutine(enumerator);
#elif TAIKO_IL2CPP
            GetMonoBehaviour().StartCoroutine(enumerator);
#endif
        }

        public void LogDebugInstance(string value)
        {
#if DEBUG
            Log.LogInfo("[DEBUG]" + value);
#endif
        }
        public static void LogDebug(string value)
        {
            Instance.LogDebugInstance(value);
        }

        public void LogInfoInstance(string value, int detailLevel = 0)
        {
            if (ConfigLoggingEnabled.Value && (ConfigLoggingDetailLevelEnabled.Value >= detailLevel))
            {
                Log.LogInfo("[" + detailLevel + "] " + value);
            }
        }
        public static void LogInfo(string value, int detailLevel = 0)
        {
            Instance.LogInfoInstance(value, detailLevel);
        }


        public void LogWarningInstance(string value, int detailLevel = 0)
        {
            if (ConfigLoggingEnabled.Value && (ConfigLoggingDetailLevelEnabled.Value >= detailLevel))
            {
                Log.LogWarning("[" + detailLevel + "] " + value);
            }
        }
        public static void LogWarning(string value, int detailLevel = 0)
        {
            Instance.LogWarningInstance(value, detailLevel);
        }


        public void LogErrorInstance(string value, int detailLevel = 0)
        {
            if (ConfigLoggingEnabled.Value && (ConfigLoggingDetailLevelEnabled.Value >= detailLevel))
            {
                Log.LogError("[" + detailLevel + "] " + value);
            }
        }
        public static void LogError(string value, int detailLevel = 0)
        {
            Instance.LogErrorInstance(value, detailLevel);
        }

    }
}
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModSaveBackup.Patch
{
    internal class SongSelectManagerStartPatch
    {
        static bool backupComplete = false;

        [HarmonyPatch(typeof(SongSelectManager))]
        [HarmonyPatch("Start")]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPostfix]
        public static void Start_Postfix(SongSelectManager __instance)
        {
            if (!backupComplete)
            {
                var time = DateTime.Now.ToString("yyyy-MM-dd HH-mm");
                try
                {
                    var folders = Plugin.Instance.ConfigFoldersToBackup.Value;
                    var splitFolders = folders.Split('|');
                    for (int i = 0; i < splitFolders.Length; i++)
                    {
                        FileBackup.BackupFolder(splitFolders[i], time);
                    }

                    var plugins = Chainloader.PluginInfos.Values.Select(x => x.Instance).ToArray();
                    BaseUnityPlugin takotakoPlugin = null;
                    BaseUnityPlugin sanaesavePlugin = null;
                    BaseUnityPlugin danidojoPlugin = null;

                    for (int i = 0; i < plugins.Length; i++)
                    {
                        if (plugins[i].Info.Metadata.GUID.ToLower() == "com.fluto.takotako".ToLower())
                        {
                            takotakoPlugin = plugins[i];
                        }
                        else if (plugins[i].Info.Metadata.GUID.ToLower() == "ca.sanae.saves".ToLower())
                        {
                            sanaesavePlugin = plugins[i];
                        }
                        else if (plugins[i].Info.Metadata.GUID.ToLower() == "com.db.DaniDojo".ToLower())
                        {
                            danidojoPlugin = plugins[i];
                        }
                    }

                    if (Plugin.Instance.ConfigBackupTakoTakoSave.Value && takotakoPlugin != null)
                    {
                        var configDef = new ConfigDefinition("CustomSongs", "SaveDirectory");
                        var value = takotakoPlugin.Config[configDef].GetSerializedValue();
                        FileBackup.BackupFolder(value, time);
                    }
                    if (Plugin.Instance.ConfigBackupSanaeSave.Value)
                    {
                        var configDef = new ConfigDefinition("General", "SaveFolderLocation");
                        var value = sanaesavePlugin.Config[configDef].GetSerializedValue();
                        FileBackup.BackupFolder(value, time);
                    }
                    if (Plugin.Instance.ConfigBackupDaniDojoSave.Value)
                    {
                        var configDef = new ConfigDefinition("Data", "DaniDojoSaveLocation");
                        var value = danidojoPlugin.Config[configDef].GetSerializedValue();
                        FileBackup.BackupFolder(value, time);
                    }
                    backupComplete = true;
                }
                catch (Exception e)
                {
                    Plugin.LogError("ModSaveBackup Failed");
                    Plugin.LogError(e.Message);
                }
            }
        }
    }
}

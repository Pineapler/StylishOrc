using Pineapler.Utils;
using System.Reflection;
using BepInEx;
using HarmonyLib;

namespace StylishOrc;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin {
    // ==========================================================
    // GAME CONFIGURATION
    public const string PluginGuid = "com.pineapler.stylishorc";
    public const string PluginName = "StylishOrc";
    public const string PluginVersion = "1.0.0";
    // ==========================================================

    public static new Config Config { get; private set; }
    
    private void Awake() {
        Config = new Config(base.Config);
        
        Log.SetSource(Logger);
        Log.Info($"Plugin {PluginGuid} is starting");
        if (Config.ModEnabled.Value == false) {
            Log.Warning($"{PluginName} is disabled");
            return;
        }

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    }
}

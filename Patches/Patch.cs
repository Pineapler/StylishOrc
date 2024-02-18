using System;
using HarmonyLib;
using StylishOrc.Scripts;
using Object = UnityEngine.Object;

namespace StylishOrc.Patches;

[HarmonyPatch]
public class Patch {

    [HarmonyPostfix]
    [HarmonyPatch(typeof(VipMassagePlan), "Start")]
    private static void AddDropdownUI(VipMassagePlan __instance) {
        // Find an existing dropdown
        var srcObj = Object.FindObjectOfType<SexSceneRoundButton>(true);
        var btnComponent = Object.Instantiate(srcObj, __instance.transform.Find("InBody"));
        var stylishManagerObj = btnComponent.gameObject;

        var manager = stylishManagerObj.AddComponent<StylishManager>();
        manager.dropdown = stylishManagerObj.AddComponent<StylishDropdown>();
    }

    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(VipMassagePlan), "SetVC")]
    private static void RefreshClothingOptions(CGManager __instance) {
        StylishManager.OnReady(() => StylishManager.Instance.RefreshCurrentCharacter());
    }


    private static bool SelfInvoked = false;
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(VipMassageGameSystem), "SetGirlCostume", new [] {typeof(string), typeof(bool)})]
    private static bool SkipSetGirlCostume(VipMassageGameSystem __instance) {
        if (SelfInvoked) return true;
        
        if (StylishManager.Instance.IsClothingOverridden) {
            SelfInvoked = true;
            __instance.SetGirlCostume("Nude"); // weird recursion :(
            SelfInvoked = false;
            StylishManager.InvokedSetGirlCostume = false;
            
            StylishManager.Instance.ReapplySelectedClothing();
            return false;
        }
        return true;
    }

}
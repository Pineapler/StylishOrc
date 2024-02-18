using System.Collections.Generic;
using Kuro;
using Pineapler.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace StylishOrc.Scripts;

public class StylishManager : MonoBehaviour {

    public static bool InvokedSetGirlCostume;
    
    private static bool _isReady = false;
    private static UnityEvent _onReady = new();

    public static void OnReady(UnityAction callback) {
        if (_isReady) {
            callback?.Invoke();
            return;
        }

        _onReady.AddListener(callback);
    }
    
    // ========================================
    
    // TODO: find a place between massages where clothes get reset

    public static StylishManager Instance;

    public StylishDropdown dropdown;
    
    public CGCharacter currentCGCharacter;
    public GirlData currentGirlData;
    public bool IsClothingOverridden => dropdown.isDirty;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(Instance.gameObject);
        }
        
        Instance = this;
    }
    
    private void Start() {
        _isReady = true;
        _onReady?.Invoke();
        _onReady?.RemoveAllListeners();
    }

    private void OnDestroy() {
        _onReady?.RemoveAllListeners();
        _isReady = false;
    }


    public void RefreshCurrentCharacter() {
        var uniqueClientData = GameCharactersHolder.Instance.GetCurrentCharacter().uniqueClientData;
        
        currentCGCharacter = CGManager.Instance.GetCgCharacter(uniqueClientData.characterType);
        currentGirlData = DataManager.Instance.nowData._girlsData[(int)GameManager.Instance.NowClient];
       
        dropdown.ClearOptions();

        dropdown.toggle.interactable = (Plugin.Config.UnlockAll.Value || currentGirlData.FavorabilityLevels >= 3);
        dropdown.toggle.isOn = false;
        
        for(int i = 0; i < currentCGCharacter.changeableCloths.Count; i++) {
            dropdown.AddOption(currentCGCharacter.changeableCloths[i], currentCGCharacter.gameObject);
        }
    }

    
    public void ReapplySelectedClothing() {
        dropdown.ReapplyToggleState();
    }


}
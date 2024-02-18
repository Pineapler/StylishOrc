using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StylishOrc.Scripts;

// Stripped down version of SexSceneRoundButton.cs
public class StylishDropdown : MonoBehaviour, IDeselectHandler, IEventSystemHandler {
    public Toggle toggle;
    public GameObject togglePrefab;
    public GameObject panel;
    public List<SexClothToggleButton> clothToggleButtons;

    public bool isDirty;
    
    private void Awake() {
        // Steal references from SexSceneRoundButton
        SexSceneRoundButton copy = GetComponent<SexSceneRoundButton>();
        if (copy == null) return;
        toggle = copy.toggle;
        togglePrefab = copy.togglePrefab;
        panel = copy.panel;
        clothToggleButtons = copy.sexClothToggleButtons;
        Destroy(copy);
        
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(ShowUI);
    }

    public void ShowUI(bool _bool) => panel.SetActive(_bool);

    public void AddOption(ClothData clothData, GameObject currentCharacter) {
        var newButton = Instantiate(togglePrefab, panel.transform).GetComponent<SexClothToggleButton>();
        newButton.Setup(clothData, currentCharacter);
        newButton.toggle.onValueChanged.AddListener(OnButtonChanged);
        clothToggleButtons.Add(newButton);
    }

    public void OnButtonChanged(bool _bool) {
        if (!isDirty) {
            isDirty = true;
            StylishManager.InvokedSetGirlCostume = true;
            VipMassageGameSystem.Instance.SetGirlCostume("Nude");
            StylishManager.InvokedSetGirlCostume = false;
            ReapplyToggleState();
        }
    }
    
    public void ClearOptions() {
        isDirty = false;
        
        foreach (var clothToggleButton in clothToggleButtons) {
            Destroy(clothToggleButton.gameObject);
        }
        clothToggleButtons.Clear();
    }
        
    public void OnDeselect(BaseEventData eventData) {
        if (EventSystem.current.IsPointerOverGameObject()) {
            if (!EventSystem.current.currentSelectedGameObject != gameObject) {
                return;
            }

            toggle.isOn = false;
        }
        else {
            toggle.isOn = false;
        }
    }

    public void ReapplyToggleState() {
        foreach (var button in clothToggleButtons) {
            button.toggle.onValueChanged?.Invoke(button.toggle.isOn);
        }
    }
}
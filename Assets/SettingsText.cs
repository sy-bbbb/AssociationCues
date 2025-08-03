using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsText : MonoBehaviour
{
    [SerializeField] ConditionManager conditionManager;
    private TextMeshProUGUI settingsText;
    // Start is called before the first frame update
    void OnEnable()
    {
        settingsText = GetComponentInChildren<TextMeshProUGUI>();
        //settingsText.text = $"Condition : {conditionManager.CurrentCondition}";
    }

}

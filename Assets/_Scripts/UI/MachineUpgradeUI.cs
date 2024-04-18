using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MachineUpgradeUI : MonoBehaviour {

    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI costText;

    private int currentUprageCost;

    private UpgradeType upgradeType;
    private Machine myMachine;

    public void Setup(Machine machine, UpgradeType upgradeType) {
        this.upgradeType = upgradeType;
        myMachine = machine;
    }

    private void Awake() {
        upgradeButton.onClick.AddListener(() => { });

    }

    private void Start() {
        costText.text = currentUprageCost.ToString();
    }


}
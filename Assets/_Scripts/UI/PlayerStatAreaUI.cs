using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum UpgradeType { Speed, Capacity, Income }

public class PlayerStatAreaUI : MonoBehaviour { //adını upgrade yap

    public static event Action<OnUpgradeEventData> OnUpgrade;

    public struct OnUpgradeEventData {
        public UpgradeType type;
        public int upgradeLevel;
    }

    [SerializeField] private UpgradeType upgradeType;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Transform colorContainer;

    private List<Image> indexImages;
    private int upgradeIndex;

    private void Awake() {

        indexImages = new List<Image>();
        foreach (Transform imageTransform in colorContainer) {
            var image = imageTransform.GetComponent<Image>();
            indexImages.Add(image);
        }

        upgradeButton.onClick.AddListener(() => TryUpgradeStat());
        UpdateButtonText();
    }

    private void TryUpgradeStat() {

        int totalUpgradeNum = indexImages.Count;

        if (upgradeIndex > totalUpgradeNum - 1) {
            Debug.Log($"Upgrade {upgradeType} is full");
            return;
        }

        if (!CanAfford()) {
            Debug.Log("Not enough money for this upgrade");
            return; }

        if (upgradeIndex < totalUpgradeNum - 1) {
            UpgradeStat();
            UpdateButtonText();
        }
        else UpgradeStat();  //upgradeIndex == totalUpgradeNum - 2
    }

    private void UpgradeStat() {
        indexImages[upgradeIndex].color = Color.green;
        upgradeIndex++;
        OnUpgrade?.Invoke(new OnUpgradeEventData {
            type = upgradeType,
            upgradeLevel = upgradeIndex
        });
    }

    private bool CanAfford() {
        int cost = DataManager.Instance.Costs[upgradeIndex];
        int money = DataManager.Instance.Money;
        return money >= cost;

    }

    private void UpdateButtonText() {
        upgradeButton.transform.GetChild(0)
            .GetComponent<TextMeshProUGUI>().text = "$ " + DataManager.Instance.Costs[upgradeIndex];
    }
}
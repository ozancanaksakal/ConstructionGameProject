using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {
    public static DataManager Instance { get; private set; }

    public event Action OnMoneyChanged;

    [SerializeField] private PrefabsSO prefabsSO;

    public PrefabsSO PrefabsSO => prefabsSO;
    public int Money { get; private set; }
    public Dictionary<UpgradeType, int> UpgradeConstants { get; private set; }
    
    public readonly int[] Costs = { 10, 20, 30, 40 };

    private int unitIncome;

    private void Awake() {
        Instance = this;
        Money = 100;
        unitIncome = 2;
        UpgradeConstants = new Dictionary<UpgradeType, int>() { { UpgradeType.Speed, 1 },
            { UpgradeType.Capacity,2} , {UpgradeType.Income,1 } };
        prefabsSO.HandleMaterialArray();
    }

    private void Start() {
        PlayerStatAreaUI.OnUpgrade += PlayerStatAreaUI_OnUpgrade;
        LockZone.OnBuy += (zone) => ChangeMoney(-zone.OpenPrice);
    }
    private void OnDestroy() {
        PlayerStatAreaUI.OnUpgrade -= PlayerStatAreaUI_OnUpgrade;
        LockZone.OnBuy -= (zone) => ChangeMoney(-zone.OpenPrice);
    }

    private void PlayerStatAreaUI_OnUpgrade(PlayerStatAreaUI.OnUpgradeEventData data) {
        if (data.type == UpgradeType.Income) {
            unitIncome += UpgradeConstants[UpgradeType.Income];
        }

        int upgradeIndex = data.upgradeLevel;
        int cost = Costs[upgradeIndex];
        ChangeMoney(-cost);
    }

    private void ChangeMoney(int amount) {
        Money += amount;
        OnMoneyChanged?.Invoke();
    }

    public void IncreaseMoney() => ChangeMoney(unitIncome);

    public BuildingMaterial GetBuildingMaterialPrefab(BuildingMaterial.Type type) =>
        prefabsSO.GetMaterialPrefab(type);
}
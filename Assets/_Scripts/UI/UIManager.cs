using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] private GameObject maxTextGameObject;
    [SerializeField] private Button playerUpgradeButton;
    [SerializeField] private Button closePlayerUpgradeaPanelButton;
    [SerializeField] private GameObject playerUpgradePanel;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Transform machineUpgradeContainer; // not yet assinged from editor
    [SerializeField] private MachineUpgradeUI machineUpgradeUIPrefab;

    public static UIManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
        maxTextGameObject.SetActive(false);
        playerUpgradePanel.SetActive(false);
        UpdateMoneyText();

        playerUpgradeButton.onClick.AddListener(() => {
            playerUpgradePanel.SetActive(true);
        });
        closePlayerUpgradeaPanelButton.onClick.AddListener(() => {
            playerUpgradePanel.SetActive(false);
        });

    }

    private void Start() {
        DataManager.Instance.OnMoneyChanged += UpdateMoneyText;
    }

    public void ShowMaxText(bool isShow) {
        maxTextGameObject.SetActive(isShow);
    }
    private void UpdateMoneyText() {
        int money = DataManager.Instance.Money;
        moneyText.text = money.ToString();
    }

    public void ShowMachineUpgradeUI(Machine machine) {
        MachineUpgradeUI machineUpgradeUI = Instantiate(machineUpgradeUIPrefab, machineUpgradeContainer);

        machineUpgradeUI.Setup(machine,UpgradeType.Speed);

        machineUpgradeUI = Instantiate(machineUpgradeUIPrefab, machineUpgradeContainer);
        machineUpgradeUI.Setup(machine,UpgradeType.Capacity);

    }
}

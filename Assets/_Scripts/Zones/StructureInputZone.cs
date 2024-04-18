using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StructureInputZone : MonoBehaviour, IInteractable {
    public event Action<StructureInputZone> OnMaterialsCollected;
    //public static event Action OnAnyMaterialPlaced;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Image materialImage;

    private int currentMaterialAmount;
    private BuildingMaterial.Type desiredInput;
    private int desiredAmount;

    public void Setup(Structure.MaterialInputData data) {
        desiredInput = data.matType;
        desiredAmount = data.amount;

        Sprite sprite = DataManager.Instance.GetBuildingMaterialPrefab(desiredInput).Sprite;
        materialImage.sprite = sprite;
    }

    private void Start() {
        currentMaterialAmount = 0;
        UpdateAmountText();
        //LookAtCamera();
    }

    public void Interact(Player player) {

        if (!player.HasMaterial()) return;

        int index = player.MaterialList.FindLastIndex(mat => mat.MatType == desiredInput);
        if (index == -1) return;

        var material = player.GiveMaterial(index);
        Destroy(material.gameObject);

        currentMaterialAmount++;
        DataManager.Instance.IncreaseMoney();

        if (currentMaterialAmount == desiredAmount) {
            OnMaterialsCollected?.Invoke(this);
            Destroy(gameObject);
            return;
        }
        UpdateAmountText();
    }

    private void UpdateAmountText() {
        string text = $"{currentMaterialAmount}/{desiredAmount}";
        amountText.text = text;
    }

    private void LookAtCamera() {
        var backgroundTransform = materialImage.transform.parent;
        Vector3 dirToCamera = (Camera.main.transform.position - backgroundTransform.position).normalized;
        backgroundTransform.LookAt(transform.position + dirToCamera * -1);
    }
}
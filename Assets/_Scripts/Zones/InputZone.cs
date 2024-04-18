using UnityEngine;

public class InputZone : MaterialZone {

    public void Setup(BuildingMaterial.Type inputMaterialType, OutputZone outputZone) {
        zoneMaterialType = inputMaterialType;

        if (zoneMaterialType == BuildingMaterial.Type.None) {
            Destroy(gameObject);
            return;
        }
        outputZone.OnProcessStarted += DestroyLastMaterial;
    }

    public override void Interact(Player player) {

        if (!HasSpace() || !player.HasMaterial() ) return;


        int index = player.MaterialList.FindLastIndex(mat => mat.MatType == zoneMaterialType);
        if (index == -1) return;

        var desiredMat = player.GiveMaterial(index);

        AddMaterialToList(desiredMat);
        desiredMat.transform.eulerAngles = Vector3.zero;
    }

    private void DestroyLastMaterial() {
        var material = materialList[^1];
        materialList.RemoveAt(materialList.Count - 1);
        Destroy(material.gameObject);
    }
}
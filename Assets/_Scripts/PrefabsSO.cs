using System;
using UnityEngine;

[CreateAssetMenu]
public class PrefabsSO : ScriptableObject {

    [SerializeField] private BuildingMaterial[] buildingMaterialArray;


    public void HandleMaterialArray() {
        Array.Sort(buildingMaterialArray, (mat1, mat2) =>
        ((int)mat1.MatType).CompareTo((int)(mat2.MatType)));
    }

    public BuildingMaterial GetMaterialPrefab(BuildingMaterial.Type type) {
        int index = (int)type;
        return buildingMaterialArray[index];
    }
}

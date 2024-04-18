using System;
using UnityEngine;

public class Structure : MonoBehaviour {
    [Serializable]
    public struct MaterialInputData {
        public BuildingMaterial.Type matType;
        public int amount;
    }

    [SerializeField] private StructureInputZone zonePrefab;
    [SerializeField] private GameObject structurePrefab;
    [SerializeField] private MaterialInputData[] materialInputDatas;

    private int nextMaterialIndex;


    private void Start() {
        CreateZone();
    }

    private void CreateZone() {

        StructureInputZone zone = Instantiate(zonePrefab, transform);
        zone.OnMaterialsCollected += Zone_OnMaterialsCollected;

        var data = materialInputDatas[nextMaterialIndex];

        zone.Setup(data);
        nextMaterialIndex++;
    }

    private void Zone_OnMaterialsCollected(StructureInputZone zone) {
        zone.OnMaterialsCollected -= Zone_OnMaterialsCollected;

        if (nextMaterialIndex != materialInputDatas.Length)
            CreateZone();
        else {
            Instantiate(structurePrefab, transform);
            //Destroy(this);
        }

    }
}

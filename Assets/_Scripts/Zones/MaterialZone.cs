using System.Collections.Generic;
using UnityEngine;

public abstract class MaterialZone : MonoBehaviour, IInteractable {

    protected List<BuildingMaterial> materialList;
    protected BuildingMaterial.Type zoneMaterialType;

    private int totalCapacity;

    private int capacityPerLayer;
    private float zoneWidthHeight;
    private Vector3 bottomLeft; // first position on grid

    private void Awake() {
        materialList = new List<BuildingMaterial>();
        Vector3 scale = GetComponentInChildren<MeshCollider>().transform.localScale;
        zoneWidthHeight = scale.x - 0.6f; // a square shape

        var material = BuildingMaterial.GetPrefab(zoneMaterialType);
        (int row, int column, int layer) = material.ZoneGrid;
        CalculateFirstPosition( row, column);

        capacityPerLayer = row * column;
        totalCapacity = capacityPerLayer * layer;
    }

    public abstract void Interact(Player player);

    protected Vector3 FindPositionToMaterial(BuildingMaterial bMat) {
        int totalMatNumber = materialList.Count;
        int row = bMat.ZoneGrid.row;
        int column = bMat.ZoneGrid.column;
        float offset = bMat.BodySize.y * 1.4f;

        int currentLayer = totalMatNumber / capacityPerLayer;
        int ObjNumInLastLayer = totalMatNumber % capacityPerLayer;

        int d = (ObjNumInLastLayer / column); // d is division
        int r = (ObjNumInLastLayer % column); // r is remainder
        float L = zoneWidthHeight;  // L is lenght

        float xPos = L * d / row;
        float zPos = L * r / column;

        Vector3 pos = new(bottomLeft.x + xPos,
           bMat.BodySize.y / 2 + currentLayer * offset,
            bottomLeft.z + zPos);

        return pos;
    }

    private void CalculateFirstPosition(int row, int column) {
        float L = zoneWidthHeight;

        bottomLeft = new(-L / 2 + L / (2 * row),
            0,
            -L / 2 + L / (2 * column));
    }

    public bool HasMaterial() => materialList.Count > 0;
    protected bool HasSpace() => materialList.Count != totalCapacity;

    protected void AddMaterialToList(BuildingMaterial bMat) {
        bMat.transform.parent = transform;
        bMat.transform.localPosition = FindPositionToMaterial(bMat);
        materialList.Add(bMat);
    }
}
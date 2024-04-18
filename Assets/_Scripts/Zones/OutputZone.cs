using System;
using UnityEngine;

public class OutputZone : MaterialZone {

    public event Action OnProcessStarted;

    private bool isInfinite;
    private InputZone inputZone;

    private float processTime;
    private float processTimeMax;

    public void Setup(BuildingMaterial outputMaterial, InputZone inputZone) {
        
        this.inputZone = inputZone;

        isInfinite = outputMaterial.DesiredInputType == BuildingMaterial.Type.None;
        zoneMaterialType = outputMaterial.MatType;
        processTimeMax = outputMaterial.ProduceTime;
    }

    private void Update() {

        if (processTime > 0) {
            processTime -= Time.deltaTime;
            if (processTime <= 0) SpawnMaterial();
            return;
        }

        if (HasInputMaterial() && HasSpace())
            StartProcess();
    }

    private void StartProcess() {
        processTime = processTimeMax;
        OnProcessStarted?.Invoke();
    }

    private bool HasInputMaterial() => isInfinite || inputZone.HasMaterial();

    private void SpawnMaterial() {
        var bMat = BuildingMaterial.CreateInstance(zoneMaterialType);
        AddMaterialToList(bMat);
    }

    public override void Interact(Player player) {
        if (materialList.Count == 0) return;
        if (!player.HasSpace()) return;

        BuildingMaterial material = materialList[^1];

        player.AddMaterial(material);

        materialList.RemoveAt(materialList.Count - 1);
    }
}
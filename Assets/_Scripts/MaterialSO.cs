using System;
using UnityEngine;

[CreateAssetMenu]
public class MaterialSO : ScriptableObject {

    public BuildingMaterial.Type type;
    public BuildingMaterial.Type inputType;
    public Sprite sprite;
    public float collectTime = 0.2f;
    public float produceTime = 0.5f;
    public Vector3 rotationOnPlayer;
    public Vector3Int onZoneRowColumnLayer;

}
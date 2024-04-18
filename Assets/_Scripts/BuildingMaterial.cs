using UnityEngine;

public class BuildingMaterial : MonoBehaviour {

    public enum Type {
        None, Log, Wood, RoofTileInput, RoofTile,
        Sand, Glass, Window, IronBar, Metal
    };
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Type type;
    [SerializeField] private Type desiredInputType;
    [SerializeField] private Sprite sprite;
    [SerializeField] private float collectTime = 0.2f;
    [SerializeField] private float produceTime = 0.5f;
    [SerializeField] private Vector3 rotationOnPlayer;
    [SerializeField] private Vector3Int zoneGrid;
    public Type MatType => type;
    public Type DesiredInputType => desiredInputType;
    public Sprite Sprite => sprite;
    public float CollectTime => collectTime;
    public float ProduceTime => produceTime;
    public Vector3 RotationOnPlayer => rotationOnPlayer;

    public (int row, int column, int layer) ZoneGrid => (zoneGrid.x, zoneGrid.y, zoneGrid.z);
    public Vector3 BodySize { get; protected set; }

    private void Awake() {

        Vector3 boundSize = type != Type.None ?
            meshFilter.mesh.bounds.size : Vector3.zero;

        Vector3 scale = transform.localScale;
        //Debug.Log($"name: {name}, size: {boundSize}");
        BodySize = new(boundSize.x * scale.x,
            boundSize.y * scale.y,
            boundSize.z * scale.z);
    }

    public static BuildingMaterial GetPrefab(Type type) {
        return DataManager.Instance.PrefabsSO.GetMaterialPrefab(type);
    }

    public static BuildingMaterial CreateInstance(Type type) {
        BuildingMaterial prefab = GetPrefab(type);
        return Instantiate(prefab);
    }
}
using UnityEngine;

public class Machine : MonoBehaviour {

    //[SerializeField] private InputZone inputZone;
    //[SerializeField] private OutputZone outputZone;
    [SerializeField] private BuildingMaterial.Type outputType;

    public BuildingMaterial.Type OutputType => outputType;

    public float Speed {  get; private set; }
    public int SpeedLevelIndex { get; private set; }
    public int CapacityLevelIndex { get; private set; }

    public InputZone InputZone { get; private set; }
    public OutputZone OutputZone { get; private set; }


    private void Awake() {
        if (outputType == BuildingMaterial.Type.None)
            Debug.LogError($"{gameObject.name} Machine output type can't be None ");
        Speed = 1f;
    }

    private void Start() {
        InputZone = GetComponentInChildren<InputZone>(true);
        OutputZone = GetComponentInChildren<OutputZone>(true);
        var outputMaterial = BuildingMaterial.GetPrefab(outputType);
        if (outputMaterial == null) Debug.Log("material null");
        SetupZones(outputMaterial);
        GetComponent<MachineVisual>().Setup(OutputZone, outputMaterial);
    }

    private void SetupZones(BuildingMaterial outputMaterial) {
        
        InputZone.Setup(outputMaterial.DesiredInputType, OutputZone);
        OutputZone.Setup(outputMaterial, InputZone);
    }

    //private void LockZone_OnBuy(LockZone obj) {
    //    if(obj.transform.parent == transform) {
    //        GetComponent<MachineVisual>().Setup(outputZone, outputMaterial);
    //    }
    //}

}
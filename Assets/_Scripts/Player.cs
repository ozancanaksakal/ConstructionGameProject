using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //public event Action<bool> OnFull;

    [SerializeField] LayerMask interactiveZoneLMask;
    [SerializeField] Transform backTransform;

    [SerializeField] float speed = 5f;
    private float rotSpeed = 8;
    private int maxCapacity = 15;

    private List<BuildingMaterial> materialList;
    bool waitBeforeTake = false;

    private float baseSpeed;
    private int baseCapacity;

    public List<BuildingMaterial> MaterialList { get { return materialList; } }

    private void Awake() {
        materialList = new List<BuildingMaterial>();
        baseSpeed = speed;
        baseCapacity = maxCapacity;

        backTransform.gameObject.SetActive(false);
    }

    private void Start() {
        PlayerStatAreaUI.OnUpgrade += PlayerStatAreaUI_OnUpgrade;
    }

    private void OnDestroy() {
        PlayerStatAreaUI.OnUpgrade -= PlayerStatAreaUI_OnUpgrade;
    }

    private void PlayerStatAreaUI_OnUpgrade(PlayerStatAreaUI.OnUpgradeEventData data) {
        var upgradeType = data.type;
        if (upgradeType == UpgradeType.Speed) {
            float speedConstant = DataManager.Instance.UpgradeConstants[UpgradeType.Speed];
            speed = baseSpeed + data.upgradeLevel * speedConstant;
        }
        else if (upgradeType == UpgradeType.Capacity) {
            int capacityConstant = DataManager.Instance.UpgradeConstants[UpgradeType.Capacity];
            maxCapacity = baseCapacity + capacityConstant * data.upgradeLevel;
        }
    }

    void Update() {
        HandleMovement();
        if (!waitBeforeTake)
            CheckForZones();
    }

    private void HandleMovement() {
        Vector2 input = InputManager.GetMovementInput();
        Vector3 input3D = new(input.x, 0, input.y);
        transform.position += speed * Time.deltaTime * input3D;
        transform.forward = Vector3.Slerp(transform.forward, input3D, Time.deltaTime * rotSpeed);
    }

    private void CheckForZones() {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, 5f, interactiveZoneLMask)) {

            var zone = hitInfo.transform.parent.GetComponent<IInteractable>();
            zone.Interact(this);
        }
    }

    private void HandleMaterialPosition(BuildingMaterial bMat) {
        Vector3 newObjPos;

        if (materialList.Count == 0)
            newObjPos = backTransform.position;
        else {
            var lastObjPos = materialList[^1].gameObject.transform.position;
            newObjPos = new(lastObjPos.x,
                lastObjPos.y + bMat.BodySize.y,
                lastObjPos.z);
        }
        bMat.transform.parent = backTransform;
        bMat.transform.SetPositionAndRotation(newObjPos, backTransform.rotation);
        bMat.transform.Rotate(bMat.RotationOnPlayer);
        //Debug.Log(newObjPos+" count: "+materialList.Count);
    }

    IEnumerator WaitBeforeNewObject(float time) {
        waitBeforeTake = true;
        yield return new WaitForSeconds(time);
        waitBeforeTake = false;
    }

    private void RearrangePositionsFrom(int index) {
        for (int i = index; i < materialList.Count; i++) {
            var material = materialList[i];
            HandleMaterialPosition(material);
        }
    }

    public void AddMaterial(BuildingMaterial material) {
        HandleMaterialPosition(material);
        materialList.Add(material);
        StartCoroutine(WaitBeforeNewObject(material.CollectTime));

        if (!backTransform.gameObject.activeSelf) backTransform.gameObject.SetActive(true);

        if (!HasSpace()) UIManager.Instance.ShowMaxText(true);
    }

    public BuildingMaterial GiveMaterial(int listIndex) {
        var desiredMat = materialList[listIndex];
        materialList.RemoveAt(listIndex);
        RearrangePositionsFrom(listIndex);
        StartCoroutine(WaitBeforeNewObject(desiredMat.CollectTime));

        UIManager.Instance.ShowMaxText(false);

        if (!HasMaterial()) backTransform.gameObject.SetActive(false);

        return desiredMat;
    }

    public bool HasMaterial() => materialList.Count > 0;
    public bool HasSpace() => materialList.Count != maxCapacity;
}
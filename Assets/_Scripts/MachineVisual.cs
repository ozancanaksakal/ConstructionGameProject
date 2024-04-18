using System.Collections;
using UnityEngine;

public class MachineVisual : MonoBehaviour {

    [SerializeField] private Transform refPoint;

    private Animator machineAnimator;
    private int animTrigger;
    private float animTime = 1f;

    private BuildingMaterial[] animationMaterials; // index => 0: input, 1: output

    private void Awake() {
        machineAnimator = GetComponent<Animator>();
        animTrigger = Animator.StringToHash("Process");
    }

    public void Setup(OutputZone outputZone, BuildingMaterial outputMaterial) {
        outputZone.OnProcessStarted += OutputZone_OnProcessStarted;
        HandleAnimationGameObjects(outputMaterial);
        animTime = animationMaterials[1].ProduceTime;
        machineAnimator.speed = 1 / animTime;
    }

    private void HandleAnimationGameObjects(BuildingMaterial outputMaterial) {

        animationMaterials = new BuildingMaterial[] {
            BuildingMaterial.CreateInstance(outputMaterial.DesiredInputType),
            BuildingMaterial.CreateInstance(outputMaterial.MatType) };

        foreach (var material in animationMaterials) {
            material.transform.parent = refPoint;
            material.transform.localPosition = Vector3.up * material.BodySize.y / 2;
            material.gameObject.SetActive(false);
        }
    }

    private void OutputZone_OnProcessStarted() {
        var inputMaterial = animationMaterials[0];
        inputMaterial.gameObject.SetActive(true);
        machineAnimator.SetTrigger(animTrigger);

        Invoke(nameof(HideObject), animTime);
    }

    private void HideObject() {
        var outputMaterial = animationMaterials[1];
        outputMaterial.gameObject.SetActive(false);
    }

    public void SwitchBuildingMaterial() { // for animation event
        animationMaterials[0].gameObject.SetActive(false);
        animationMaterials[1].gameObject.SetActive(true);
    }
}
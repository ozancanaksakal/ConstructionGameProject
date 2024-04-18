using UnityEngine;

public class MachineUpgradeZone : InteractiveZone {

    // Class is not complete

    [SerializeField] private Machine myMachine;

    protected override void OnWaitComplete() {
        UIManager.Instance.ShowMachineUpgradeUI(myMachine);
    }
}
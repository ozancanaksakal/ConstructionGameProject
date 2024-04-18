using UnityEngine;

public class SellZone : MonoBehaviour, IInteractable {


    public void Interact(Player player) {


        if (player.HasMaterial()) {
            var lastMaterial = player.GiveMaterial(player.MaterialList.Count - 1);

            Destroy(lastMaterial.gameObject);

            DataManager.Instance.IncreaseMoney();
        }
    }
}

using System;
using UnityEngine;

public class LockZone : InteractiveZone, IInteractable {

    public static event Action<LockZone> OnBuy;
    [SerializeField] private int openPrice = 20;

    public int OpenPrice => openPrice;

    private void Start() {

        if (openPrice == 0) {
            Destroy(gameObject);
            return;
        }

    }

    protected override void OnWaitComplete() {
        if (openPrice < DataManager.Instance.Money) {
            OnSuccess();
        }
        else { OnFail(); }
    }

    private void OnFail() {
        quadRenderer.material.color = Color.red;
        Invoke(nameof(SetBackToStart), 1);
    }

    private void OnSuccess() {
            OnBuy?.Invoke(this);
        int deneme = 0;
        foreach (Transform child in transform.parent) {
            child.gameObject.SetActive(true);
            deneme++;
        }
        Destroy(gameObject);
    }
}

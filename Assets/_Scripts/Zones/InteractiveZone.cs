using System.Collections;
using UnityEngine;

public abstract class InteractiveZone : MonoBehaviour, IInteractable {

    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] protected MeshRenderer quadRenderer;

    private readonly float desiredWaitTime = 2f;
    private float waitTime;

    private bool isInteractPlayer;

    private Vector3 zoneCheckSize;
    private Color initColor;
    private void Awake() {
        var scale = quadRenderer.transform.lossyScale* .85f;
        zoneCheckSize = new(scale.x, 0.2f, scale.z);

        initColor = quadRenderer.material.color;

    }
    public void Interact(Player player) {
        if (!isInteractPlayer) { StartCoroutine(PlayerCheck()); }
    }

    private IEnumerator PlayerCheck() {
        isInteractPlayer = true;
        bool isWaitCompleted = true;
        waitTime = Time.time + desiredWaitTime;
        while (Time.time < waitTime) {
            bool isHit = Physics.BoxCast(transform.position - Vector3.up / 2, zoneCheckSize,
                Vector3.up, Quaternion.identity, 2, playerLayerMask);

            if (isHit) {
                float lerp = Mathf.PingPong(waitTime, desiredWaitTime);
                quadRenderer.material.color = Color.Lerp(initColor, Color.green, lerp);
            }
            else {
                isWaitCompleted = false;
                SetBackToStart();
                break;
            }
            yield return null;
        }

        if (isWaitCompleted) { OnWaitComplete(); }
    }

    protected abstract void OnWaitComplete();

    protected virtual void SetBackToStart() {
        waitTime = 0;
        isInteractPlayer = false;
        quadRenderer.material.color = initColor;
    }

}
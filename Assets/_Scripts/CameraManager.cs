using UnityEngine;

public class CameraManager : MonoBehaviour {

    private Vector3 offset;
    private Transform referenceTransform;

    private void Awake() {
        offset = transform.position;

        referenceTransform = FindAnyObjectByType<Player>().transform;
    }

    private void LateUpdate() {
        transform.position = offset + referenceTransform.position;
    }
}

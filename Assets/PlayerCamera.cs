using UnityEngine;
public enum CameraState {
    FollowPlayer,
    Fixed,
    None
}
public class PlayerCamera : MonoBehaviour {
    public Player target;
    public static float defaultFollowSize = 5.5f;
    public float followSize = 5.5f;
    public float fixedSize = 5.5f;
    public CameraState state = CameraState.FollowPlayer;
    public Transform fixedTarget;
    Camera cam;
    public float speed = 15;
    void Start() {
        cam = GetComponent<Camera>();
    }
    void FixedUpdate() {
        if (state == CameraState.FollowPlayer) {
            if (!target) return;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, followSize, 10 * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);
        } else if (state == CameraState.Fixed) {
            if (!fixedTarget) return;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, fixedSize, 10 * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, fixedTarget.transform.position, speed * Time.deltaTime);
        } else {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, defaultFollowSize, 10 * Time.deltaTime);
        }
    }

}
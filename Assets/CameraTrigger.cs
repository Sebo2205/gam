using UnityEngine;
public class CameraTrigger : MonoBehaviour {
    public CameraState state;
    public float fixedSize = 5.5f;
    public float followSize = 5.5f;
    public Transform fixedTarget;
    void OnTriggerStay2D(Collider2D col) {
        Player p = col.GetComponent<Player>();
        if (p) {
            Debug.Log("Camera enter");
            p.cam.state = state;
            p.cam.fixedSize = fixedSize;
            p.cam.followSize = followSize;
            p.cam.fixedTarget = fixedTarget;
        }
    }
    void OnTriggerExit2D(Collider2D col) {
        Player p = col.GetComponent<Player>();
        if (p) {
            Debug.Log("Camera exit");
            p.cam.state = CameraState.FollowPlayer;
            p.cam.fixedSize = 5.5f;
            p.cam.followSize = PlayerCamera.defaultFollowSize;
            p.cam.fixedTarget = null;
        }    
    }
}
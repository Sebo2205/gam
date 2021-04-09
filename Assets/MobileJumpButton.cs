using UnityEngine;
using UnityEngine.UI;
public class MobileJumpButton : MonoBehaviour {
    public Player target;
    void Start() {
        if (SystemInfo.deviceType != DeviceType.Handheld && !Input.touchSupported) {
            Destroy(gameObject);
            return;
        }
    }
    public void Jump() {
        Debug.Log("ha ha yes");
        if (!target) return;
        target.doJump = true;
    }
}
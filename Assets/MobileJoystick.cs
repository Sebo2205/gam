using UnityEngine;
using UnityEngine.UI;
public class MobileJoystick : MonoBehaviour {
    public RectTransform outer;
    public Player target;
    RectTransform r;
    bool h = false;
    void Start() {
        if (SystemInfo.deviceType != DeviceType.Handheld && !Input.touchSupported) {
            Destroy(gameObject);
            return;
        }
        r = GetComponent<RectTransform>();
    }
    void Update() {
        if (Input.GetMouseButtonDown(0) && outer.rect.Contains(Input.mousePosition)) h = true;
        if (Input.GetMouseButtonUp(0)) h = false;
        Vector2 movement = ((Vector2)r.position - outer.rect.center) / r.rect.width;
        Debug.Log(movement);
        if (h) {
            Vector2 p = Input.mousePosition;
            r.position = new Vector2(Mathf.Clamp(p.x, outer.position.x, outer.position.x + outer.rect.width), Mathf.Clamp(p.y, outer.position.y, outer.position.y + outer.rect.height));
        } else r.position = (Vector2)outer.position + (outer.rect.size / 2);
        if (target) {
            target.movement = movement.x;
            target.vertical = movement.y;
        }
    }
}
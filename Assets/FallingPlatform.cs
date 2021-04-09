using UnityEngine;
public class FallingPlatform : MonoBehaviour {
    public float fallTime = 1;
    float fTime;
    bool falling = false;
    Rigidbody2D rb;
    Vector3 defaultPos;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        fTime = fallTime;
        defaultPos = transform.position;
    }
    void Update() {
        if (falling) {
            fallTime -= Time.deltaTime;
            if (fallTime <= 0) rb.isKinematic = false;
            if (rb.isKinematic) {
                transform.position = Vector3.Lerp(defaultPos, defaultPos - (Vector3.up * .25f), 1 - (fallTime / fTime));
            }
        }
    }
    void OnCollisionEnter2D(Collision2D col) {
        if (falling) return;
        Rigidbody2D r = col.collider.GetComponent<Rigidbody2D>();
        if (r) {
            falling = true;
        }
    }
}
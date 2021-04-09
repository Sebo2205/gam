using UnityEngine;
public class DamageTrigger : MonoBehaviour {
    public float damage = 1;
    void OnTriggerStay2D(Collider2D col) {
        Health h = col.GetComponent<Health>();
        if (h) {
            h.TakeDamage(damage);
        }
    }
}
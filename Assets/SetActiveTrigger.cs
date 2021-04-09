using UnityEngine;
public class SetActiveTrigger : MonoBehaviour {
    public GameObject target;
    public bool value;
    void OnTriggerEnter2D(Collider2D col) {
        Player p = col.GetComponent<Player>();
        if (p) {
            target.SetActive(value);
        }
    }
}
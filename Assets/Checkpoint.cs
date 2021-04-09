using UnityEngine;
public class Checkpoint : MonoBehaviour {
    public int id = 0;
    void OnTriggerEnter2D(Collider2D col) {
        Player p = col.GetComponent<Player>();
        if (p) {
            if (id >= p.checkpoint) {
                p.checkpoint = id;
                p.respawnPos = transform.position;
                p.health = p.maxHealth;
            }
        }
    }
}
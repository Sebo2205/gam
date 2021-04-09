using UnityEngine;
public class LevelWarp : MonoBehaviour {
    public Stage target;
    bool didThing = false;
    void OnTriggerEnter2D(Collider2D col) {
        if (didThing) return;
        Player p = col.GetComponent<Player>();
        if (p) {
            target.LoadStatic();
        }
    }
}
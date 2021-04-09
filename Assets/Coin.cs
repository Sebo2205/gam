using UnityEngine;
public class Coin : MonoBehaviour {
    public int amount = 1;
    void OnTriggerEnter2D(Collider2D col) {
        Player p = col.GetComponent<Player>();
        if (p) {
            p.coins += amount;
            if (p.coins >= 100) {
                p.coins -= 100;
                StageLoader.lives++;
                StageLoader.main.UpdateLifeCounter();
            }
            StageLoader.main.UpdateCoinCounter();
            Destroy(gameObject);
        }
    }
}
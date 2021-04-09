using UnityEngine;
public class Thing : MonoBehaviour {
    public bool collected = false;
    public int id;
    StageLoader.StageThing s;
    void Start() {
        if (!StageLoader.main) {
            Destroy(gameObject);
            return;
        }
        if (!StageLoader.main.curStage) {
            Destroy(gameObject);
            return;
        }
        s = StageLoader.main.stageThings[StageLoader.main.curStage.stageNumber];
        if (!s.firstLoad) return;
        s.thingCount++;
    }
    void Update() {
        if (s.thingsGot == null) return;
        if (s.thingsGot[id]) {
            collected = true;
            gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D col) {
        Player p = col.GetComponent<Player>();
        if (p && !collected) {
            s.thingsGot[id] = true;
            collected = true;
            gameObject.SetActive(false);
            StageLoader.main.UpdateThings();
        }
    }
}
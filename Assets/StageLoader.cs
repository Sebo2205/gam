using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class StageLoader : MonoBehaviour {
    [System.Serializable]
    public class StageThing {
        public Stage stage;
        public bool[] thingsGot;
        public bool firstLoad = true;
        public int thingCount = 0;
        public bool exists = false;
        public StageThing(Stage stage) {
            this.stage = stage;
        }
    }
    public GameObject loadingScreen;
    public GameObject spawnOnStart;
    public static int lives = 3;
    public StageThing[] stageThings;
    public Stage curStage;
    public Stage defaultStage;
    SpawnOnStart spawn;
    public static StageLoader main;
    public void UpdateLifeCounter() {
        spawn.lifeCounter.text = lives.ToString("00");
    }
    public void UpdateCoinCounter() {
        spawn.coinCounter.text = spawn.player.coins.ToString();
    }
    void Start() {
        DontDestroyOnLoad(gameObject);
        stageThings = new StageThing[100];
        main = this;
        LoadStage(defaultStage);
    }
    public void LoadStage(Stage s, System.Nullable<Vector2> respawnPos = null) {
        StartCoroutine(_LoadStage(s, respawnPos));
    }
    public void PlayerRespawn(Player p) {
        StartCoroutine(_PlayerRespawn(p));
    }
    IEnumerator _PlayerRespawn(Player p) {
        spawn.hudAnim.SetTrigger("Close");
        if (lives > 0) {
            Vector2 respawnPos = p.respawnPos;
            Time.timeScale = 1;
            lives--;
            yield return new WaitForSeconds(1);
            Destroy(p.gameObject);
            LoadStage(curStage, respawnPos);
            yield return null;
        } else {
            Time.timeScale = 1;
            lives = 3;
            yield return new WaitForSeconds(2);
            Destroy(p.gameObject);
            LoadStage(defaultStage);
            yield return null;
        }
    }
    public void UpdateThings() {
        int i = 0;
        foreach (GameObject g in spawn.things) {
            if (stageThings[curStage.stageNumber] == null || stageThings[curStage.stageNumber].thingsGot == null) break;
            if (i < stageThings[curStage.stageNumber].thingCount) {
                g.SetActive(true);
                if (stageThings[curStage.stageNumber].thingsGot[i] == true) g.transform.GetChild(0).gameObject.SetActive(true);
                else g.transform.GetChild(0).gameObject.SetActive(false);
            } else {
                g.SetActive(false);
            }
            i++;
        }
    }
    IEnumerator _LoadStage(Stage s, System.Nullable<Vector2> respawnPos = null) {
        Time.timeScale = 1;
        StageThing t = stageThings[s.stageNumber];
        if (t == null || !t.exists) t = stageThings[s.stageNumber] = new StageThing(s);
        t.exists = true;
        StageInfo.main = null;
        spawn = null;
        curStage = null;
        GameObject loading = Instantiate(loadingScreen);
        LoadingScreen ls = loading.GetComponent<LoadingScreen>();
        if (!ls) {
            Destroy(loading);
            LoadStage(s);
        } else {
            ls.stageNumber.text = $"STAGE {s.stageNumber.ToString("00")}";
            ls.stageName.text = s.stageName;
            DontDestroyOnLoad(loading);
            yield return new WaitForSeconds(.6f);
            AsyncOperation op = SceneManager.LoadSceneAsync(s.buildIndex);
            curStage = s;
            while (!op.isDone) {
                ls.loadingBar.fillAmount = op.progress / 0.9f;
                yield return null;
            }
            SpawnOnStart p = Instantiate(spawnOnStart).GetComponent<SpawnOnStart>();
            p.hudAnim.gameObject.SetActive(false);
            spawn = p;
            if (t.firstLoad) {
                t.thingsGot = new bool[8];
                t.firstLoad = false;
                Debug.Log($"Thing count: {t.thingCount}");
            }
            p.player.inputEnabled = false;
            if (respawnPos == null) {
                p.player.transform.position = StageInfo.main.spawn.position;
                p.player.respawnPos = StageInfo.main.spawn.position;
            } else {
                p.player.transform.position = (Vector3)respawnPos;
                p.player.respawnPos = (Vector2)respawnPos;
            }
            p.lifeCounter.text = lives.ToString("00");
            yield return new WaitForSeconds(.7f);
            ls.anim.SetTrigger("Close");
            yield return new WaitForSeconds(.7f);
            UpdateThings();
            p.hudAnim.gameObject.SetActive(true);
            p.hudAnim.SetTrigger("Open");
            p.player.inputEnabled = true;
            Destroy(loading);
            yield return null;
        }
    }
}
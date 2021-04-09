using UnityEngine;
public class StageInfo : MonoBehaviour {
    public Stage stage;
    public static StageInfo main;
    public Transform spawn;
    void Start() {
        main = this;
    }
}
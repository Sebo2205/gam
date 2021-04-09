using UnityEngine;
[CreateAssetMenu( fileName = "New Stage", menuName = "Thing/Stage" )]
public class Stage : ScriptableObject {
    public int buildIndex;
    public int stageNumber;
    public string stageName;
    public void Load(StageLoader loader) {
        Debug.Log($"Sending to stage {stageNumber}: {stageName}");
        loader.LoadStage(this);
    }
    [ContextMenu("Force load")]
    public void LoadStatic() {
        if (StageLoader.main) Load(StageLoader.main);
        else Debug.Log("The main stage loader has not been set");
    }
}
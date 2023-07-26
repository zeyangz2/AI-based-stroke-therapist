using UnityEngine;

[CreateAssetMenu(fileName = "SavePaths", menuName = "ScriptableObjects/SavePathsScriptableObject", order = 1)]
public class SavePathsScriptableObject : ScriptableObject
{
    public string CSVPath;
    public string VideoPath;
}
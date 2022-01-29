using UnityEngine;
using UnityEditor;

public class PatternEditor : EditorWindow
{
    static int lenX;
    static int lenY;

    static bool[,] pattern;

    [MenuItem("Window/Pattern Editor")]
    public static void ShowWindow()
    {
        GetWindow<PatternEditor>("Pattern Editor");
    }

    void OnGUI()
    {   
        GUILayout.BeginHorizontal();
        GUILayout.Label("Pattern Size :", GUILayout.ExpandWidth(false));
        lenX = EditorGUILayout.IntField(lenX, GUILayout.Width(50));
        lenY = EditorGUILayout.IntField(lenY, GUILayout.Width(50));
        GUILayout.EndHorizontal();

        if(pattern == null) pattern = new bool[lenX,lenY];
        if(pattern.Length != lenX * lenY) pattern = new bool[lenX,lenY];

        for(int y = 0; y < lenY; y++){
            GUILayout.BeginHorizontal();
            for(int x = 0; x < lenX; x++){
                pattern[x,y] = EditorGUILayout.Toggle(pattern[x,y], GUILayout.Width(20));
            }
            GUILayout.EndHorizontal();
        }

        if(GUILayout.Button("Save Pattern", GUILayout.Width(100))){

            string path = "Assets/Patterns";
            if(!AssetDatabase.IsValidFolder(path)){
                System.IO.Directory.CreateDirectory(path);
            }
            
            PatternSO asset = ScriptableObject.CreateInstance<PatternSO>();
            asset.pattern = pattern;

            AssetDatabase.CreateAsset(asset, path+"/new pattern.asset");
            AssetDatabase.SaveAssets();
            
            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
        GUILayout.Label("Change name before creating new one\nIf a pattern already exists at default path it will be deleted prior to creating a new pattern");
    }

}

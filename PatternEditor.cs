using UnityEngine;
using UnityEditor;

public enum PatternType{
    Bool,
    Int,
    Float,
    String
}

enum AreaSelection{
    New,
    Edit
}
public class PatternEditor : EditorWindow
{
    static int lenX = 2;
    static int lenY = 2;
    static string path;
    static string fileName = "new pattern";
    static PatternType patternType = PatternType.Bool;
    static bool[,] boolPattern;
    static int[,] intPattern;
    static float[,] floatPattern;
    static string[,] stringPattern;

    static TextAsset json;
    static bool[,] editboolPattern;
    static int[,] editintPattern;
    static float[,] editfloatPattern;
    static string[,] editstringPattern;
    
    //editor
    static AreaSelection areaSelection = AreaSelection.New;
    static int fieldWidth = 20;
    static int fieldHeight = 20;
    static bool focusAfterSave = false;
    Vector2 patternScrollPos;

    [MenuItem("Window/Pattern Editor")]
    public static void ShowWindow()
    {
        GetWindow<PatternEditor>("Pattern Editor");
    }
    void OnGUI()
    {   
        int padding = 10;
        int width = (int)(Screen.width * 0.95f) - padding;
        int height = (int)(Screen.height * 0.95f) - padding;

        int leftWidth = 220;
        int leftPosY = padding;
        int selectionHeight = 25;
        int saveHeight = 100;
        

        int editPosX = padding * 2 + leftWidth;
        int editWidth = width - editPosX;


        GUILayout.BeginArea(new Rect(padding, leftPosY, leftWidth, selectionHeight), BackgroundColor(new Color(0,0,0,0)));
        GUISelection(leftWidth);
        GUILayout.EndArea();
        leftPosY += padding + selectionHeight;
        int settingHeight = height - leftPosY - padding - saveHeight;
        switch(areaSelection){
            case AreaSelection.New:
                GUILayout.BeginArea(new Rect(padding, leftPosY, leftWidth, settingHeight), BackgroundColor(new Color(1,1,1,0.05f)));
                GUINewPatternSetting();
                GUILayout.EndArea();
                leftPosY += padding + settingHeight;
                break;
            case AreaSelection.Edit:
                GUILayout.BeginArea(new Rect(padding, leftPosY, leftWidth, settingHeight), BackgroundColor(new Color(1,1,1,0.05f)));
                GUIEditPatternSetting();
                GUILayout.EndArea();
                leftPosY += padding + settingHeight;
                break;
        }
        

        int lineWidth = 5;
        GUILayout.BeginArea(new Rect(editPosX, padding, lineWidth, height), BackgroundColor(new Color(0,0,0,0.1f)));
        GUILayout.EndArea();

        editPosX += padding + lineWidth;

        GUILayout.BeginArea(new Rect(editPosX, padding, editWidth, height));
        patternScrollPos = GUILayout.BeginScrollView(patternScrollPos);
        GUIPatternEdit();
        GUILayout.EndScrollView();
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(padding, height - saveHeight, leftWidth, saveHeight), BackgroundColor(new Color(0,0,0,0)));
        GUIPatternSave(leftWidth, saveHeight);
        GUILayout.EndArea();
    }
    GUIStyle BackgroundColor(Color color){
        GUIStyle style = new GUIStyle();
        Texture2D texture = new Texture2D(1,1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        style.normal.background = texture;
        return style;
    }
    void GUISelection(int width){
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("New", GUILayout.Width(width / 2 - 5))){
            path = null;
            json = null;
            patternType = PatternType.Bool;
            boolPattern = null;
            intPattern = null;
            floatPattern = null;
            stringPattern = null;
            lenX = 2;
            lenY = 2;
            areaSelection = AreaSelection.New;
        }
        if(GUILayout.Button("Edit", GUILayout.Width(width / 2 - 5))){
            path = null;
            boolPattern = null;
            intPattern = null;
            floatPattern = null;
            stringPattern = null;
            lenX = 0;
            lenY = 0;
            areaSelection = AreaSelection.Edit;
        }
        GUILayout.EndHorizontal();
    }
    void GUIEditorSetting(){
        GUILayout.Label("Editor input:", GUILayout.ExpandWidth(false));

        GUILayout.BeginHorizontal();
        GUILayout.Label(" Height\t\t", GUILayout.ExpandWidth(false));
        fieldHeight = EditorGUILayout.IntField(fieldHeight, GUILayout.Width(50));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label(" Width\t\t", GUILayout.ExpandWidth(false));
        fieldWidth = EditorGUILayout.IntField(fieldWidth, GUILayout.Width(50));
        GUILayout.EndHorizontal();
    }
    void GUINewPatternSetting(){
        GUIEditorSetting();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Pattern name\t", GUILayout.ExpandWidth(false));
        fileName = EditorGUILayout.TextField(fileName, GUILayout.Width(103));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Pattern type\t", GUILayout.ExpandWidth(false));
        patternType = (PatternType)EditorGUILayout.EnumPopup(patternType, GUILayout.Width(103));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Pattern size\t", GUILayout.ExpandWidth(false));
        lenX = EditorGUILayout.IntField(lenX, GUILayout.Width(50));
        lenY = EditorGUILayout.IntField(lenY, GUILayout.Width(50));
        GUILayout.EndHorizontal();
        
        switch(patternType){
            case PatternType.Bool:
                boolPattern = setPattern(boolPattern);
                break;
            case PatternType.Int:
                intPattern = setPattern(intPattern);
                break;
            case PatternType.Float:
                floatPattern = setPattern(floatPattern);
                break;
            case PatternType.String:
                stringPattern = setPattern(stringPattern);
                break;
        }


    }
    void GUIEditPatternSetting(){
        GUIEditorSetting();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Pattern File\t", GUILayout.ExpandWidth(false));
        json = (TextAsset)EditorGUILayout.ObjectField(json, typeof(TextAsset), false, GUILayout.Width(103));
        GUILayout.EndHorizontal();
        (string, bool) pathData = setPath(path, json);
        
        path = pathData.Item1;

        if(json != null && pathData.Item2){
            System.Type jsonType = getPatternType(json.text);

            if(jsonType == typeof(bool)){
                patternType = PatternType.Bool;
            }else if(jsonType == typeof(int)){
                patternType = PatternType.Int;
            }else if(jsonType == typeof(float)){
                patternType = PatternType.Float;
            }else if(jsonType == typeof(string)){
                patternType = PatternType.String;
            }else{
                Debug.LogWarning("Unsupported Pattern Type");
            }

            switch(patternType){
                case PatternType.Bool:
                    bool[,] _boolPattern = dataToPattern<bool>(json.text);
                    lenX = _boolPattern.GetLength(0);
                    lenY = _boolPattern.GetLength(1);
                    boolPattern = _boolPattern;
                    break;
                case PatternType.Int:
                    int[,] _intPattern = dataToPattern<int>(json.text);
                    lenX = _intPattern.GetLength(0);
                    lenY = _intPattern.GetLength(1);
                    intPattern = _intPattern;
                    break;
                case PatternType.Float:
                    float[,] _floatPattern = dataToPattern<float>(json.text);
                    lenX = _floatPattern.GetLength(0);
                    lenY = _floatPattern.GetLength(1);
                    floatPattern = _floatPattern;
                    break;
                case PatternType.String:
                    string[,] _stringPattern = dataToPattern<string>(json.text);
                    lenX = _stringPattern.GetLength(0);
                    lenY = _stringPattern.GetLength(1);
                    stringPattern = _stringPattern;
                    break;
            }
        }
    }
    void GUIPatternEdit(){
        for(int y = 0; y < lenY; y++){
            GUILayout.BeginHorizontal();
            for(int x = 0; x < lenX; x++){

                switch(patternType){
                    case PatternType.Bool:
                        boolPattern[x,y] = EditorGUILayout.Toggle(boolPattern[x,y], GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                        break;
                    case PatternType.Int:
                        intPattern[x,y] = EditorGUILayout.IntField(intPattern[x,y],GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                        break;
                    case PatternType.Float:
                        floatPattern[x,y] = EditorGUILayout.FloatField(floatPattern[x,y], GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                        break;
                    case PatternType.String:
                        stringPattern[x,y] = EditorGUILayout.TextField(stringPattern[x,y], GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                        break;
                }
                
            }
            GUILayout.EndHorizontal();
        }
    }
    void GUIPatternSave(int width, int height){
        
        if(GUILayout.Button("Save Pattern", GUILayout.Width(width-6))){

            if(areaSelection == AreaSelection.New){
                path = "Assets/Patterns";
                if(!AssetDatabase.IsValidFolder(PatternEditor.path)){
                    System.IO.Directory.CreateDirectory(PatternEditor.path);
                }
                
                path += "/"+patternType;
                if(!AssetDatabase.IsValidFolder(PatternEditor.path)){
                    System.IO.Directory.CreateDirectory(PatternEditor.path);
                }

                path += "/"+lenX+"x"+lenY;
                if(!AssetDatabase.IsValidFolder(PatternEditor.path)){
                    System.IO.Directory.CreateDirectory(PatternEditor.path);
                }

                path += "/"+fileName+".json";
            }
            
            switch(patternType){
                case PatternType.Bool:
                    WriteToJsonFile(path, JsonUtility.ToJson(patternToData(boolPattern, PatternType.Bool)));
                break;
                case PatternType.Int:
                    WriteToJsonFile(path, JsonUtility.ToJson(patternToData(intPattern, PatternType.Int)));
                    break;
                case PatternType.Float:
                    WriteToJsonFile(path, JsonUtility.ToJson(patternToData(floatPattern, PatternType.Float)));
                    break;
                case PatternType.String:
                    WriteToJsonFile(path, JsonUtility.ToJson(patternToData(stringPattern, PatternType.String)));
                    break;
            }
            AssetDatabase.Refresh();

            if(focusAfterSave){
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            }
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("Focus file after save", GUILayout.ExpandWidth(false));
        focusAfterSave = EditorGUILayout.Toggle( focusAfterSave);
        GUILayout.EndHorizontal();

        GUIStyle style = EditorStyles.wordWrappedLabel;
        style.normal.textColor = new Color(0.75f, 0.75f, 0);
        style.fontSize = 11;
        GUILayout.TextArea("Change name before creating new one.\nIf a pattern already exists at default path it will be deleted prior to creating a new pattern.", style);
    }
    (string, bool) setPath(string path_, TextAsset asset){
        string assetPath = AssetDatabase.GetAssetPath(json);
        return (path_ != null)?(assetPath, !path_.Equals(assetPath)):(assetPath, true);
    }

    Type[,] setPattern<Type>(Type[,] self, Type[,] pattern){
        if(self == null)self = pattern;
        if(self.Length != lenX * lenY) self = pattern;
        return self;
    }
    Type[,] setPattern<Type>(Type[,] pattern){
        if(pattern == null) pattern = new Type[lenX, lenY];
        if(pattern.Length != lenX * lenY) pattern = new Type[lenX, lenY];
        return pattern;
    }
    private void WriteToJsonFile(string path, string jsonText){
        System.IO.FileStream fileStream = new System.IO.FileStream(path, System.IO.FileMode.Create);
        using(System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(fileStream)){
            streamWriter.Write(jsonText);
        }
    }
    public static System.Type getPatternType(string jsonText){
        PatternData<object> patternData = JsonUtility.FromJson<PatternData<object>>(jsonText);
        switch(patternData.patternType){
            case PatternType.Bool:
                return typeof(bool);
            case PatternType.Int:
                return typeof(int);
            case PatternType.Float:
                return typeof(float);
            case PatternType.String:
                return typeof(string);

            default:
                return null;
        }
    }
    public static Type[,] dataToPattern<Type>(string jsonText){
        PatternData<Type> patternData = JsonUtility.FromJson<PatternData<Type>>(jsonText);
        int dataLenR = patternData.row.Length;
        int dataLenC = patternData.row[0].column.Length;
        
        Type[,] pattern = new Type[dataLenC, dataLenR];

        for(int y = 0; y < dataLenR; y++){
            for(int x = 0; x < dataLenC; x++){
                pattern[x, y] = patternData.row[y].column[x].value;
            }
        }

        return pattern;
    }
    PatternData<Type> patternToData<Type>(Type[,] pattern, PatternType patternType){
        PatternData<Type> patternData = new PatternData<Type>();
        patternData.patternType = patternType;
        patternData.row = new PatternRow<Type>[pattern.GetLength(1)];
        for(int y = 0; y < pattern.GetLength(1); y++){
            PatternRow<Type> row = new PatternRow<Type>();
            row.column = new PatternColumn<Type>[pattern.GetLength(0)];
            for(int x = 0; x < pattern.GetLength(0); x++){
                PatternColumn<Type> column = new PatternColumn<Type>();
                column.value = pattern[x, y];
                row.column[x] = column;
            }
            patternData.row[y] = row;
        }
        return patternData;
    }

    [System.Serializable]
    public class PatternData<Type>{
        public PatternType patternType;
        public PatternRow<Type>[] row; 
    }
    [System.Serializable]
    public class PatternRow<Type>{
        public PatternColumn<Type>[] column;
    }
    [System.Serializable]
    public class PatternColumn<Type>{
        public Type value; 
    }

}

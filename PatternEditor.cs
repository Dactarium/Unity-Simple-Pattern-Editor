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
    
    static bool[] boolPatternVertical;
    static bool[] boolPatternHorizontal;
    static int[] intPatternVertical;
    static int[] intPatternHorizontal;
    static float[] floatPatternVertical;
    static float[] floatPatternHorizontal;
    static string[] stringPatternVertical;
    static string[] stringPatternHorizontal;

    static bool boolPatternAll;
    static int intPatternAll;
    static float floatPatternAll;
    static string stringPatternAll;


    static TextAsset json;
    
    // Editor Settings
    static AreaSelection areaSelection = AreaSelection.New;
    static int fieldWidth = 20;
    static int fieldHeight = 20;
    static bool focusAfterSave = false;
    Vector2 patternScrollPos;

    [MenuItem("Window/Simple Pattern Editor")]
    public static void ShowWindow()
    {
        GetWindow<PatternEditor>("Pattern Editor");
    }
    void OnGUI()
    {   
        int padding = 10;
        int width = (int)(Screen.width * 0.95f) - padding;
        int height = (int)(Screen.height * 0.95f) - padding;

        int leftWidth = 221;
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
            boolPatternVertical = null;
            intPatternVertical = null;
            floatPatternVertical = null;
            stringPatternVertical = null;
            boolPatternHorizontal = null;
            intPatternHorizontal = null;
            floatPatternHorizontal = null;
            stringPatternHorizontal = null;
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
            boolPatternVertical = null;
            intPatternVertical = null;
            floatPatternVertical = null;
            stringPatternVertical = null;
            boolPatternHorizontal = null;
            intPatternHorizontal = null;
            floatPatternHorizontal = null;
            stringPatternHorizontal = null;
            lenX = 0;
            lenY = 0;
            areaSelection = AreaSelection.Edit;
        }
        GUILayout.EndHorizontal();
    }
    void GUIEditorSetting(){
        GUILayout.Label("Editor input:", GUILayout.ExpandWidth(false));

        GUILayout.BeginHorizontal();
        GUILayout.Label(" Width\t\t", GUILayout.ExpandWidth(false));
        fieldWidth = EditorGUILayout.IntSlider(fieldWidth, 15, 100, GUILayout.Width(105));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label(" Height\t\t", GUILayout.ExpandWidth(false));
        fieldHeight = EditorGUILayout.IntSlider(fieldHeight, 15, 100, GUILayout.Width(105));
        GUILayout.EndHorizontal();
       
    }
    void GUINewPatternSetting(){
        GUIEditorSetting();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Pattern name\t", GUILayout.ExpandWidth(false));
        fileName = EditorGUILayout.TextField(fileName, GUILayout.Width(105));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Pattern type\t", GUILayout.ExpandWidth(false));
        patternType = (PatternType)EditorGUILayout.EnumPopup(patternType, GUILayout.Width(105));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Pattern size\t", GUILayout.ExpandWidth(false));
        lenX = EditorGUILayout.IntField(lenX, GUILayout.Width(51));
        lenY = EditorGUILayout.IntField(lenY, GUILayout.Width(51));
        GUILayout.EndHorizontal();
        
        switch(patternType){
            case PatternType.Bool:
                boolPattern = setPatternArray(boolPattern);
                boolPatternVertical = setPatternSideArray(boolPatternVertical, true);
                boolPatternHorizontal = setPatternSideArray(boolPatternHorizontal, false);
                break;
            case PatternType.Int:
                intPattern = setPatternArray(intPattern);
                intPatternVertical = setPatternSideArray(intPatternVertical, true);
                intPatternHorizontal = setPatternSideArray(intPatternHorizontal, false);
                break;
            case PatternType.Float:
                floatPattern = setPatternArray(floatPattern);
                floatPatternVertical = setPatternSideArray(floatPatternVertical, true);
                floatPatternHorizontal = setPatternSideArray(floatPatternHorizontal, false);
                break;
            case PatternType.String:
                stringPattern = setPatternArray(stringPattern);
                stringPatternVertical = setPatternSideArray(stringPatternVertical, true);
                stringPatternHorizontal = setPatternSideArray(stringPatternHorizontal, false);
                break;
        }


    }
    void GUIEditPatternSetting(){
        GUIEditorSetting();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Pattern File\t", GUILayout.ExpandWidth(false));
        json = (TextAsset)EditorGUILayout.ObjectField(json, typeof(TextAsset), false, GUILayout.Width(105));
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

                    boolPatternVertical = setPatternSideArray(boolPatternVertical, true);
                    boolPatternHorizontal = setPatternSideArray(boolPatternHorizontal, false);
                    break;
                case PatternType.Int:
                    int[,] _intPattern = dataToPattern<int>(json.text);
                    lenX = _intPattern.GetLength(0);
                    lenY = _intPattern.GetLength(1);
                    intPattern = _intPattern;

                    intPatternVertical = setPatternSideArray(intPatternVertical, true);
                    intPatternHorizontal = setPatternSideArray(intPatternHorizontal, false);
                    break;
                case PatternType.Float:
                    float[,] _floatPattern = dataToPattern<float>(json.text);
                    lenX = _floatPattern.GetLength(0);
                    lenY = _floatPattern.GetLength(1);
                    floatPattern = _floatPattern;

                    floatPatternVertical = setPatternSideArray(floatPatternVertical, true);
                    floatPatternHorizontal = setPatternSideArray(floatPatternHorizontal, false);
                    break;
                case PatternType.String:
                    string[,] _stringPattern = dataToPattern<string>(json.text);
                    lenX = _stringPattern.GetLength(0);
                    lenY = _stringPattern.GetLength(1);
                    stringPattern = _stringPattern;

                    stringPatternVertical = setPatternSideArray(stringPatternVertical, true);
                    stringPatternHorizontal = setPatternSideArray(stringPatternHorizontal, false);
                    break;
            }
        }
    }
    void GUIPatternEdit(){
        GUILayout.BeginHorizontal();

        editAll();

        GUILayout.Space(fieldWidth);

        for(int x = 0; x < lenX; x++){
            editVeritcal(x);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(fieldHeight);

        for(int y = 0; y < lenY; y++){
            GUILayout.BeginHorizontal();

            editHorizontal(y);

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
                EditorGUIUtility.PingObject(Selection.activeObject);
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

    void editAll(){
        switch(patternType){
            case PatternType.Bool:
                bool newBoolValue = EditorGUILayout.Toggle(boolPatternAll, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                if(newBoolValue != boolPatternAll){
                    boolPatternAll = newBoolValue;

                    for(int y = 0; y < lenY; y++){
                        boolPatternHorizontal[y] = newBoolValue;
                        for(int x = 0; x < lenX; x++){
                            boolPattern[x,y] = newBoolValue;
                        }
                    }

                    for(int x = 0; x < lenX; x++){
                        boolPatternVertical[x] = newBoolValue;
                    }
                }
                break;
            case PatternType.Int:
                int newIntValue = EditorGUILayout.IntField(intPatternAll, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                if(newIntValue != intPatternAll){
                    intPatternAll = newIntValue;

                    for(int y = 0; y < lenY; y++){
                        intPatternHorizontal[y] = newIntValue;
                        for(int x = 0; x < lenX; x++){
                            intPattern[x,y] = newIntValue;
                        }
                    }

                    for(int x = 0; x < lenX; x++){
                        intPatternVertical[x] = newIntValue;
                    }
                }
                break;
            case PatternType.Float:
                float newFloatValue = EditorGUILayout.FloatField(floatPatternAll, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                if(newFloatValue != floatPatternAll){
                    floatPatternAll = newFloatValue;

                    for(int y = 0; y < lenY; y++){
                        floatPatternHorizontal[y] = newFloatValue;
                        for(int x = 0; x < lenX; x++){
                            floatPattern[x,y] = newFloatValue;
                        }
                    }

                    for(int x = 0; x < lenX; x++){
                        floatPatternVertical[x] = newFloatValue;
                    }
                }
                break;
            case PatternType.String:
                string newStringValue = EditorGUILayout.TextField(stringPatternAll, GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                if(newStringValue != stringPatternAll){
                    stringPatternAll = newStringValue;

                    for(int y = 0; y < lenY; y++){
                        stringPatternHorizontal[y] = newStringValue;
                        for(int x = 0; x < lenX; x++){
                            stringPattern[x,y] = newStringValue;
                        }
                    }

                    for(int x = 0; x < lenX; x++){
                        stringPatternVertical[x] = newStringValue;
                    }
                }
                break;
        }
    }

    void editVeritcal(int x){
        switch(patternType){
            case PatternType.Bool:
                bool newBoolValue = EditorGUILayout.Toggle(boolPatternVertical[x], GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                if(newBoolValue != boolPatternVertical[x]){
                    boolPatternVertical[x] = newBoolValue;

                    for(int y = 0; y < lenY; y++){
                        boolPattern[x,y] = newBoolValue;
                    }
                }
                break;

            case PatternType.Int:
                int newIntValue = EditorGUILayout.IntField(intPatternVertical[x],GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                if(newIntValue != intPatternVertical[x]){
                    intPatternVertical[x] = newIntValue;

                    for(int y = 0; y < lenY; y++){
                        intPattern[x,y] = newIntValue;
                    }
                }
                break;

            case PatternType.Float:
                float newFloatValue = EditorGUILayout.FloatField(floatPatternVertical[x], GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                if(newFloatValue != floatPatternVertical[x]){
                    floatPatternVertical[x] = newFloatValue;

                    for(int y = 0; y < lenY; y++){
                        floatPattern[x,y] = newFloatValue;
                    }
                }
                break;

            case PatternType.String:
                string newStringValue = EditorGUILayout.TextField(stringPatternVertical[x], GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                if(newStringValue != stringPatternVertical[x]){
                    stringPatternVertical[x] = newStringValue;

                    for(int y = 0; y < lenY; y++){
                        stringPattern[x,y] = newStringValue;
                    }
                }
                break;
        }
    }

    void editHorizontal(int y){
        switch(patternType){
            case PatternType.Bool:
                bool newBoolValue = EditorGUILayout.Toggle(boolPatternHorizontal[y], GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                if(newBoolValue != boolPatternHorizontal[y]){
                    boolPatternHorizontal[y] = newBoolValue;

                    for(int x = 0; x < lenX; x++){
                        boolPattern[x,y] = newBoolValue;
                    }
                }
                GUILayout.Space(fieldWidth);
                break;

            case PatternType.Int:
                int newIntValue = EditorGUILayout.IntField(intPatternHorizontal[y],GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                if(newIntValue != intPatternHorizontal[y]){
                    intPatternHorizontal[y] = newIntValue;

                    for(int x = 0; x < lenX; x++){
                        intPattern[x,y] = newIntValue;
                    }
                }
                GUILayout.Space(fieldWidth);
                break;

            case PatternType.Float:
                float newFloatValue = EditorGUILayout.FloatField(floatPatternHorizontal[y], GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                if(newFloatValue != floatPatternHorizontal[y]){
                    floatPatternHorizontal[y] = newFloatValue;

                    for(int x = 0; x < lenX; x++){
                        floatPattern[x,y] = newFloatValue;
                    }
                }
                GUILayout.Space(fieldWidth);
                break;

            case PatternType.String:
                string newStringValue = EditorGUILayout.TextField(stringPatternHorizontal[y], GUILayout.Width(fieldWidth), GUILayout.Height(fieldHeight));
                if(newStringValue != stringPatternHorizontal[y]){
                    stringPatternHorizontal[y] = newStringValue;

                    for(int x = 0; x < lenX; x++){
                        stringPattern[x,y] = newStringValue;
                    }
                }
                GUILayout.Space(fieldWidth);
                break;
        }
    }

    Type[,] setPatternArray<Type>(Type[,] pattern){
        if(pattern == null) pattern = new Type[lenX, lenY];
        if(pattern.Length != lenX * lenY) pattern = new Type[lenX, lenY];
        return pattern;
    }

    Type[] setPatternSideArray<Type>(Type[] patternSide, bool isVertical){
        int len = (isVertical)? lenX: lenY;

        if(patternSide == null) patternSide = new Type[len];
        if(patternSide.Length != len) patternSide = new Type[len];
        return patternSide;

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
    Type[,] dataToPattern<Type>(string jsonText){
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
    class PatternData<Type>{
        public PatternType patternType;
        public PatternRow<Type>[] row; 
    }
    [System.Serializable]
    class PatternRow<Type>{
        public PatternColumn<Type>[] column;
    }
    [System.Serializable]
    class PatternColumn<Type>{
        public Type value; 
    }

}

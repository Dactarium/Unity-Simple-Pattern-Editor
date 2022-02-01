using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PatternType{
    Bool,
    Int,
    Float,
    String
}

public class PatternConverter : MonoBehaviour
{
    public static Type[,] convert<Type>(string jsonText){
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


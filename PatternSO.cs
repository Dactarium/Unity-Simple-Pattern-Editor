using UnityEngine;

public class PatternSO : ScriptableObject
{
    bool[] pattern;
    int lenX;
    int lenY;
    public void setPattern(bool[,] _pattern){
        pattern = new bool[_pattern.Length];
        lenX = _pattern.GetLength(0);
        lenY = _pattern.GetLength(1);
        for(int y = 0; y < lenY; y++){
            for(int x = 0; x < lenX; x++){
                pattern[y * lenX + x] = _pattern[x, y] == true;
            }
        }
    }
    public bool[,] getPattern(){
       bool[,] pattern_ = new bool[lenX, lenY];
        for(int y = 0; y < lenY; y++){
            for(int x = 0; x < lenX; x++){
                pattern_[x, y] = pattern[y * lenX + x];
            }
        }
       return pattern_;
    }
}


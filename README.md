# Unity Simple Pattern Editor

## Getting Started

add PatternEditor.cs and PatternSO.cs to Unity's "Assets" Folder

## Usage

Open Window/Pattern Editor

![image](https://user-images.githubusercontent.com/75855560/151671807-fb88d387-8300-4f16-bb7b-131a6ba1e683.png)

Enter pattern size

![image](https://user-images.githubusercontent.com/75855560/151671881-38512c3a-d2f6-469b-a917-514679b348bb.png)

Design pattern and click save button

![image](https://user-images.githubusercontent.com/75855560/151671929-eaedc32a-1760-4cb8-be3f-a89687464c80.png)

After clicking save button you will see new pattern (Scriptable Object) at "Assets/Patterns" folder

![image](https://user-images.githubusercontent.com/75855560/151671988-f6c33b4a-2645-4a39-ad93-58d2e181c1a3.png)

<strong>Example usage PatternSO</storng>
```c#
public PatternSO asset;
bool[,] pattern = asset.getPattern(); // Returns bool[,]
```
 

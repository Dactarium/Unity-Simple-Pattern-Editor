# Unity Simple Pattern Editor

## Getting Started

add PatternEditor.cs to Unity's "Assets" Folder

## Creating new pattern

Open Window/Pattern Editor

![image](https://user-images.githubusercontent.com/75855560/151671807-fb88d387-8300-4f16-bb7b-131a6ba1e683.png)

Enter pattern name, type and size

![image](https://user-images.githubusercontent.com/75855560/151841918-2fc16763-bb9b-4241-9d3f-ce046ff97c92.png)

Design pattern and click <strong>Save Patern</strong> button

![image](https://user-images.githubusercontent.com/75855560/151842280-098705d9-00bc-41bc-b2ee-1cad9753350f.png)

After clicking save button you will see new pattern (Json File) at "Assets/Patterns/'Pattern type'/'Pattern width'x'Pattern height'" folder
PS. If you check <strong>Focus after save</strong> you will automaticly see file you create

![image](https://user-images.githubusercontent.com/75855560/151842380-a7f526c1-e8a0-42c6-8bca-8a53d9cf8ffe.png)

## Editing exist pattern

Click <strong>Edit</strong> button Select exist pattern json

![image](https://user-images.githubusercontent.com/75855560/151844051-4105415d-ee6d-4e5e-a620-47feca6e5c35.png)

Edit pattern from edit area

![image](https://user-images.githubusercontent.com/75855560/151844210-8fc69612-cc4c-4818-a2bb-9f0f9365096b.png)

Click <strong>Save Patern</strong> button

## Example usage in Script (Bool Pattern Json)
```c#
public TextAsset json;
public bool[,] pattern = PatternEditor.dataToPattern<bool>(json.text);
```

## Supported Data Types

Boolean, Integer, Float, String

using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public DialogueSpeaker speaker;
    [TextArea(3, 10)]
    public string text;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    private Queue<DialogueLine> dialogueLines;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image image;
    public Sprite none;

    public Animator animator;

    private char[] muteCharacters = { ' ', '.', ',', '!', '?', '-', ';'};

    void Start()
    {
        dialogueLines = new Queue<DialogueLine>();
    }

    public void StartDialogue(DialogueLine[] dialogue)
    {
        animator.SetBool("IsOpen", true);

        dialogueLines.Clear();

        foreach (DialogueLine dialogueLine in dialogue)
        {
            dialogueLines.Enqueue(dialogueLine);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (dialogueLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine sentence = dialogueLines.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (DialogueLine dialogueLine)
    {
        DialogueSpeaker speaker = dialogueLine.speaker;
        if (speaker.sprite != null)
        {
            image.overrideSprite = speaker.sprite;
        } else
        {
            image.overrideSprite = none;
        }
        nameText.SetText(speaker.name);
        dialogueText.text = "";

        foreach (char letter in dialogueLine.text.ToCharArray())
        {
            dialogueText.text += letter;
            if (!muteCharacters.Contains(letter))
            {
                SoundManager.PlaySound(SoundType.TAP, 0, true);
            }
            yield return new WaitForSecondsRealtime(.03f);
        }
    }

    private void EndDialogue() {
        animator.SetBool("IsOpen", false);
    }
}

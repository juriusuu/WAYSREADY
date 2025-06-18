using UnityEngine;
using JasonSkillman.Dialogue;

public class NPCDialogueTrigger : MonoBehaviour
{
    public DialogueScript dialogueScript; // Assign your DialogueScript asset in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueScript.TriggerDialogue();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxManager : MonoBehaviour
{
    public DialogueBox[] dbArr;
    public int currIndex;
    public DialogueBox temp, current;
    // Start is called before the first frame update
    void Start()
    {
        current = GetComponent<DialogueBox>();
        temp = current;
    }

    // Update is called once per frame
    void Update()
    {
        current = temp;
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            currIndex += 1;
            UpdateDialogue();
        }
    }

    void UpdateDialogue()
    {
        temp = dbArr[currIndex];
        current = temp;
    }
}

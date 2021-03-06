﻿using UnityEngine;
using System.Collections;

public class elevatorFirstLevel : MonoBehaviour
{
    bool triggered;
    public Animator elevatorAC;

    public string[] dialogueLines; //dialogue lines to send to dialogueManager
    public string speakerName; // speaker's name

    public AudioClip buttonBeepSound;

    private DialogueManager dialogueManager;

    public AudioSource elevatorAudioSource;

    // Use this for initialization
    void Start()
    {
        triggered = false;
        this.elevatorAC = this.GetComponent<Animator>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Acitvate elevator if player is colliding and clicks E
    /// </summary>
    /// <param name="col">colliding object</param>
    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player" && !triggered)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                AudioSource.PlayClipAtPoint(this.buttonBeepSound, this.transform.position, 3.5f);
                this.elevatorAudioSource.enabled = true;
                triggered = true;
                this.elevatorAC.SetBool("triggered", true);
                dialogueManager.showDialogue(this.speakerName, this.dialogueLines, 6.0f);
            }
        }
    }
}

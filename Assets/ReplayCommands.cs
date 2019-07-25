using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayCommands : MonoBehaviour
{
    private List<Command> commands;
    private WaitForSeconds actionDelay = new WaitForSeconds(.1f);
    private WaitForFixedUpdate fixedUpdateDelay = new WaitForFixedUpdate();
    private Coroutine undoCoroutine;
    private Coroutine replayCoroutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            commands = GameManager.ListOfCommands;
            StartReplay();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            commands = GameManager.ListOfCommands;
            StartUndo();
        }
    }

    private void StartUndo()
    {
        if (commands.Count > 0)
        {
            if (undoCoroutine != null)
            {
                StopCoroutine(undoCoroutine);
            }
            undoCoroutine = StartCoroutine(_UndoCommands());
        }
    }

    private void StartReplay()
    {
        if (commands.Count > 0)
        {
            if (replayCoroutine != null)
            {
                StopCoroutine(replayCoroutine);
            }
            replayCoroutine = StartCoroutine(_ReplayCommands());
        }
    }

    private IEnumerator _UndoCommands()
    {
        for (int i = commands.Count - 1; i >= 0; i--)
        {
            commands[i].Undo(GameManager.FocusObject);
            yield return fixedUpdateDelay;
        }
    }

    private IEnumerator _ReplayCommands()
    {
        for (int i = commands.Count - 1; i >= 0; i--)
        {
            commands[i].Redo(GameManager.FocusObject);
            yield return fixedUpdateDelay;
        }
    }
}

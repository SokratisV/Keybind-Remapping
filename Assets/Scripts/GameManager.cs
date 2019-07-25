using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    private static GameObject focusObject;
    private static List<Command> listOfCommands = new List<Command>();

    public static GameObject FocusObject { get => focusObject; set => focusObject = value; }
    public static List<Command> ListOfCommands
    {
        get
        {
            return listOfCommands;
        }
        set => listOfCommands = value; }
}

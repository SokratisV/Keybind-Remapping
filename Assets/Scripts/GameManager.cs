using UnityEngine;

public static class GameManager
{
    private static GameObject focusObject;

    public static GameObject FocusObject { get => focusObject; set => focusObject = value; }
}

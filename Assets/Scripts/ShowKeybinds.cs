﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Keybinds;

public class ShowKeybinds : MonoBehaviour
{
    public Keybind_Mapping script;
    public Button button;
    public int modeOfKeybindRemapping = 1; //1 or 2

    void Start()
    {
        /*
         * Iterates through the dictionary, creates buttons, edits their text to match the keybind,
         * and attaches the Rebind method on click.
         */
        foreach (KeyValuePair<string, Command> item in script.Dict)
        {
            Button temp = Instantiate(button, gameObject.transform);
            temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Key + "-" + item.Value.ToString();
        }
        Refresh();
    }

    /* 
     * Destroys the UI Buttons in the panel, reinstantiates them and rebinds the correct on click event.
     */
    public void Refresh()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        foreach (KeyValuePair<string, Command> item in script.Dict)
        {
            Button temp = Instantiate(button, gameObject.transform);
            temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Key + "-" + item.Value.ToString();
            if (modeOfKeybindRemapping == 1)
            {
                temp.onClick.AddListener(delegate { script.Rebind(item.Key); });
            }
            else if (modeOfKeybindRemapping == 2)
            {
                temp.onClick.AddListener(delegate { script.Rebind2(item.Key); });
            }
        }
    }
}

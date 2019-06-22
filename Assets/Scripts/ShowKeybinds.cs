using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowKeybinds : MonoBehaviour
{
    public Keybind_Mapping script;
    public Button button;

    void Start()
    {
        // Iterates through the dictionary, creates buttons, edits their text to match the keybind,
        // and attaches the Rebind method on click.
        foreach (KeyValuePair<string, Command> item in script.Dict)
        {
            Button temp = Instantiate(button, gameObject.transform);
            temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Key;
            temp.onClick.AddListener(delegate { script.Rebind(item.Key); });
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Refresh();
        }
    }

    // Not working yet
    public void Refresh()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        print("Destroyed, creating new (Not working yet)");
        foreach (KeyValuePair<string, Command> item in script.Dict)
        {
            Button temp = Instantiate(button, gameObject.transform);
            temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Key;
            temp.onClick.AddListener(delegate { script.Rebind(item.Key); });
        }
    }
}

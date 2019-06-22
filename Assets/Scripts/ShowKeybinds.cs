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
        foreach (KeyValuePair<string, Command> item in script.Dict)
        {
            Button temp = Instantiate(button, gameObject.transform);
            temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.Key;
            temp.onClick.AddListener(delegate { script.Rebind(item.Key); });
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Keybind_Mapping : MonoBehaviour
{
    private Dictionary<string, Command> dict;
    private Command cmd;
    [SerializeField]
    private string bind1 = "", bind2 = "";
    private KeyCode keyCode;
    [SerializeField]
    private ShowKeybinds script;
    [SerializeField]
    private string bindToChange = "";
    [SerializeField]
    private bool editMode = false;


    public GameObject canvas;


    public Dictionary<string, Command> Dict { get => dict; }

    private void Start()
    {
        GameManager.FocusObject = GameObject.FindWithTag("Player");
    }

    void Awake()
    {
        //Add all available buttons to the dictionary
        dict = new Dictionary<string, Command>
        {
            { "W", new MoveForwards() },
            { "Space", new Jump() },
            { "Mouse1", new Attack() },
            { "F", new DoNothing() },
            { "A", new MoveLeft() },
            { "D", new MoveRight() },
            { "S", new MoveBackwards() }
        };
    }
    void Update()
    {
        if (editMode)
        {
            ChangeInput();
        }
        else
        {
            HandleInput();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            editMode = !editMode;
            canvas.SetActive(editMode);
        }
    }

    private void HandleInput()
    {
        foreach (KeyValuePair<string, Command> bind in dict)
        {
            if (Input.GetKey(keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), bind.Key)))
            {
                bind.Value.Execute(GameManager.FocusObject, bind.Value);
            }
        }
    }

    /*
* Coroutine instead of function for new thread. Removes the two entries from the dictionary,
* and adds new ones with swapped Keys. Holds and uses previous Values (Commands).
*/
    private IEnumerator _Rebind(string bind1, string bind2)
    {
        dict.TryGetValue(bind1, out Command tempCommand1);
        dict.TryGetValue(bind2, out Command tempCommand2);

        dict.Remove(bind1);
        dict.Remove(bind2);

        dict.Add(bind2, tempCommand1);
        dict.Add(bind1, tempCommand2);

        this.bind1 = "";
        this.bind2 = "";
        bindToChange = "";
        script.Refresh();
        yield return null;
    }
    /*
     *  Must be used twice in success to work. First time saves the bind (string), second time
     *  calls a coroutine with the previous and current bind. The use of 1 argument is a restriction
     *  from Unity's events which require 0 or 1 arguments.
     */
    public void Rebind(string bind)
    {
        if (bind1 == "")
        {
            bind1 = bind;
        }
        else
        {
            bind2 = bind;
            StartCoroutine(_Rebind(bind1, bind2));
        }
    }
    /*
     * Used when user clicks on button, marking the keybind they want to change.
     */
    public void Rebind2(string bind)
    {
        bindToChange = bind;
    }
    /*
     * If the user has marked a keybind to change, swaps it with the next key pressed. This key has to
     * be registered in the Dictionary (dict) above as a Key.
     */
    private void ChangeInput()
    {
        if (bindToChange != "")
        {
            // --- The three loops below do the same thing. The first one throws an exception but works
            // --- The second one does not offer access to the value (but is faster probably)

            //foreach (KeyValuePair<string, Command> bind in dict)
            //{
            //    if (Input.GetKeyDown(keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), bind.Key)))
            //    {
            //        StartCoroutine(_Rebind(bindToChange, bind.Key));
            //        bind.Value.Execute();
            //    }
            //}
            //foreach (var key in dict.Keys.ToList())
            //{
            //    if (Input.GetKeyDown(keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), key)))
            //    {
            //        StartCoroutine(_Rebind(bindToChange, key));
            //    }
            //}
            for (int index = 0; index < dict.Count; index++)
            {
                var item = dict.ElementAt(index);
                var itemKey = item.Key;
                //var itemValue = item.Value;

                if (Input.GetKeyDown(keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), item.Key)))
                {
                    if (bindToChange != item.Key)
                    {
                        StartCoroutine(_Rebind(bindToChange, item.Key));
                        //itemValue.Execute(GameManager.FocusObject);
                    }
                }
            }
        }
    }
    /*
     * Handles the input. Implemented with 2 different ways. The first, the manual way, better for
     * performance. The second focuses on automating the process of adding new keybinds by iterating through
     * and using the keys that exist in the dictionary. 
     *
    private void ChangeInput()
    {
        //-- NORMAL way, check each input manually --
        //////////////////////////////////////////////
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    dict.TryGetValue("W", out cmd);
        //    cmd.Execute();
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    dict.TryGetValue("A", out cmd);
        //    cmd.Execute();
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    dict.TryGetValue("D", out cmd);
        //    cmd.Execute();
        //}
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    dict.TryGetValue("Space", out cmd);
        //    cmd.Execute();
        //}
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    dict.TryGetValue("Mouse0", out cmd);
        //    //cmd.Execute();
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    dict.TryGetValue("S", out cmd);
        //    cmd.Execute();
        //}
        //////////////////////////////////////////////
        //-- OR for a more automated system --
        //////////////////////////////////////////////
        // Iterates through the dictionary, converts every key (string) to a KeyCode, and executes the value (Command)
        foreach (KeyValuePair<string, Command> bind in dict)
        {
            if (Input.GetKeyDown(keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), bind.Key)))
            {
                //bind.Value.Execute(GameManager.FocusObject);
            }
        }
    }*/
}

internal class MoveRight : Command
{
    public override void Execute(GameObject obj, Command command)
    {
        Debug.Log("Moving Right");
        obj.GetComponent<Rigidbody>().AddForce(Vector3.right * moveForce);
        GameManager.ListOfCommands.Add(command);
    }

    public override void Redo(GameObject obj)
    {
        Debug.Log("Re-Moving Right");
        obj.GetComponent<Rigidbody>().AddForce(Vector3.right * moveForce);
    }

    public override void Undo(GameObject obj)
    {
        Debug.Log("Un-Moving Right");
        obj.GetComponent<Rigidbody>().AddForce(-Vector3.right * moveForce);
    }
}
internal class MoveLeft : Command
{
    public override void Execute(GameObject obj, Command command)
    {
        Debug.Log("Moving Left");
        obj.GetComponent<Rigidbody>().AddForce(Vector3.left * moveForce);
        GameManager.ListOfCommands.Add(command);
    }

    public override void Redo(GameObject obj)
    {
        Debug.Log("Re-Moving Left");
        obj.GetComponent<Rigidbody>().AddForce(Vector3.left * moveForce);
    }

    public override void Undo(GameObject obj)
    {
        Debug.Log("Un-Moving Left");
        obj.GetComponent<Rigidbody>().AddForce(-Vector3.left * moveForce);
    }
}
internal class Attack : Command
{
    public override void Execute(GameObject obj, Command command)
    {
        Debug.Log("Attacking");
    }

    public override void Redo(GameObject obj)
    {
        Debug.Log("Re-Attacking");
    }

    public override void Undo(GameObject obj)
    {
        Debug.Log("Un-Attacking");
    }
}
internal class MoveForwards : Command
{
    public override void Execute(GameObject obj, Command command)
    {
        Debug.Log("MovingForward");
        obj.GetComponent<Rigidbody>().AddForce(Vector3.forward * moveForce);
        GameManager.ListOfCommands.Add(command);
    }

    public override void Redo(GameObject obj)
    {
        Debug.Log("Re-MovingForward");
        obj.GetComponent<Rigidbody>().AddForce(Vector3.forward * moveForce);
    }

    public override void Undo(GameObject obj)
    {
        Debug.Log("Un-MovingForward");
        obj.GetComponent<Rigidbody>().AddForce(-Vector3.forward * moveForce);
    }
}
internal class Jump : Command
{
    public override void Execute(GameObject obj, Command command)
    {
        Debug.Log("Jumping");
        obj.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce);
        GameManager.ListOfCommands.Add(command);
    }

    public override void Redo(GameObject obj)
    {
        Debug.Log("Re-Jumping");
        obj.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce);
    }

    public override void Undo(GameObject obj)
    {
        Debug.Log("Un-Jumping");
        obj.GetComponent<Rigidbody>().AddForce(-Vector3.up * jumpForce);
    }
}
internal class DoNothing : Command
{
    public override void Execute(GameObject obj, Command command)
    {
        Debug.Log("Doing Nothing");
    }

    public override void Redo(GameObject obj)
    {
        Debug.Log("Re-Doing Nothing");
    }

    public override void Undo(GameObject obj)
    {
        Debug.Log("Un-Doing Nothing");
    }
}
internal class MoveBackwards : Command
{
    public override void Execute(GameObject obj, Command command)
    {
        Debug.Log("Moving back");
        obj.GetComponent<Rigidbody>().AddForce(Vector3.back * moveForce);
        GameManager.ListOfCommands.Add(command);
    }

    public override void Redo(GameObject obj)
    {
        Debug.Log("Re-Moving back");
        obj.GetComponent<Rigidbody>().AddForce(Vector3.back * moveForce);
    }

    public override void Undo(GameObject obj)
    {
        Debug.Log("Undo-Moving back");
        obj.GetComponent<Rigidbody>().AddForce(-Vector3.back * moveForce);
    }
}

public abstract class Command
{
    protected float moveForce = 20f;
    protected float jumpForce = 40f;

    public abstract void Execute(GameObject obj, Command command);
    public abstract void Redo(GameObject obj);
    public abstract void Undo(GameObject obj);
}



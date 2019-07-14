using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keybinds
{
    public class Keybind_Mapping : MonoBehaviour
    {
        private Dictionary<string, Command> dict;
        private Command cmd;
        private bool editMode = false;
        [SerializeField]
        private string bind1 = "", bind2 = "";
        private KeyCode keyCode;
        [SerializeField]
        private ShowKeybinds script;

        public Dictionary<string, Command> Dict { get => dict; }

        void Awake()
        {
            //Add all available buttons to the dictionary
            dict = new Dictionary<string, Command>
        {
            { "W", new MoveForward() },
            { "Space", new Jump() },
            { "Mouse0", new Attack() },
            { "S", new DoNothing() },
            { "A", new MoveLeft() },
            { "D", new MoveRight() }
        };
        }

        void Update()
        {
            if (!editMode)
            {
                ManageInput();
            }
            else
            {
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

            /////////////--DEBUG--/////////////
            //print("Binding: " + bind2 + " to -->");
            //tempCommand1.Execute();
            //print("Binding: " + bind1 + " to -->"); 
            //tempCommand2.Execute();
            //////////////////////////////////
            this.bind1 = "";
            this.bind2 = "";
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
                script.Refresh();
            }
        }

        /*
         * Handles the input. Implemented with 2 different ways. The first, the manual way, better for
         * performance. The second focuses on automating the process of adding new keybinds by iterating through
         * and using the keys that exist in the dictionary. 
         */
        private void ManageInput()
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
                    bind.Value.Execute();
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                editMode = true;
                cmd = null;
            }
        }
    }

    internal class MoveRight : Command
    {
        public override void Execute()
        {
            Debug.Log("Moving Right");
        }
    }

    internal class MoveLeft : Command
    {
        public override void Execute()
        {
            Debug.Log("Moving Left");
        }
    }

    internal class Attack : Command
    {
        public override void Execute()
        {
            //Debug.Log("Attacking");
        }
    }
    internal class MoveForward : Command
    {
        public override void Execute()
        {
            Debug.Log("MovingForward");
        }
    }
    internal class Jump : Command
    {
        public override void Execute()
        {
            Debug.Log("Jumping");
        }
    }
    internal class DoNothing : Command
    {
        public override void Execute()
        {
            Debug.Log("Doing Nothing");
        }
    }

    public abstract class Command
    {
        public abstract void Execute();
    }
}


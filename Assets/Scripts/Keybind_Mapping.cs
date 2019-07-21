using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Keybinds
{
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

        public Dictionary<string, Command> Dict { get => dict; }

        void Awake()
        {
            //Add all available buttons to the dictionary
            dict = new Dictionary<string, Command>
        {
            { "W", new MoveForward() },
            { "Space", new Jump() },
            { "Mouse1", new Attack() },
            { "S", new DoNothing() },
            { "A", new MoveLeft() },
            { "D", new MoveRight() }
        };
        }
        void Update()
        {
            if (script.modeOfKeybindRemapping == 1)
            {
                ManageInput();
            }
            else if (script.modeOfKeybindRemapping == 2)
            {
                ManageInput2();
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
        private void ManageInput2()
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
                        StartCoroutine(_Rebind(bindToChange, item.Key));
                        //itemValue.Execute();
                    }
                }
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
            Debug.Log("Attacking");
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


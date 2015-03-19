using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    public class SuperDebug : MonoBehaviour {

        private static bool enableGUI = false;

        private static string debugInfoConsole = "";
        private static string debugInfoFixed = "";
        private static Dictionary<string, object> debugInfoFixedList = new Dictionary<string, object>();

        void Start() {

        }

        void Update() {

        }

        public static bool EnableGUI {
            get { return enableGUI; }
            set {
                if (value == true) {
                    if (GameObject.Find("DEBUG") == false) {
                        GameObject DEBUG = new GameObject();
                        DEBUG.name = "DEBUG";
                        DEBUG.AddComponent<SuperDebug>();
                    }
                } else {
                    if (GameObject.Find("DEBUG") == true) {
                        GameObject DEBUG = GameObject.Find("DEBUG");
                        Destroy(DEBUG.GetComponent<SuperDebug>());
                        Destroy(DEBUG);
                    }
                }
                enableGUI = value;
            }
        }


        private static float debugWidth = 300f;
        private static Vector2 btnSize = new Vector2(50f, 50f);

        void OnGUI() {
            if (enableGUI == true) {
                GUIStyle gs = new GUIStyle("TextArea");
                gs.alignment = TextAnchor.UpperLeft;
                GUI.TextArea(new Rect(0f, 0f, debugWidth, Screen.height), debugInfoFixed, gs);
                gs = new GUIStyle("Button");
                if (GUI.Button(new Rect(debugWidth, 0f, btnSize.x, btnSize.y), "◄◄", gs)) {
                    if (debugWidth - 50f <= 0f) {
                        debugWidth = 0f;
                    } else {
                        debugWidth -= 50f;
                    }
                }
                if (GUI.Button(new Rect(debugWidth, Screen.height - btnSize.y, btnSize.x, btnSize.y), "►►", gs)) {
                    if (debugWidth + 50f < Screen.width - 50f) {
                        debugWidth += 50f;
                    } else {
                        debugWidth = Screen.width - 50f;
                    }
                }
            }
        }

        public static void Log4Console(params object[] list) {
            for (int i = 0; i < list.Length; i++) {
                Debug.Log(list[i]);
            }
        }
        public static void Log4ConsoleOneLine(params object[] list) {
            debugInfoConsole = "";
            for (int i = 0; i < list.Length; i++) {
                if (i < list.Length - 1) {
                    debugInfoConsole += list[i] + "      ";
                } else {
                    debugInfoConsole += list[i];
                }
            }
        }

        public static void Log4GUI(params KeyValuePair<string, object>[] list) {
            if (enableGUI == false) return;
            debugInfoFixed = "";

            foreach (KeyValuePair<string, object> item in list) {
                if (debugInfoFixedList.ContainsKey(item.Key)) {
                    debugInfoFixedList[item.Key] = item.Value;
                } else {
                    debugInfoFixedList.Add(item.Key, item.Value);
                }
            }

            foreach (KeyValuePair<string, object> kvp in debugInfoFixedList) {
                debugInfoFixed += kvp.Key + " : " + kvp.Value + "\n";
            }

        }


        public static KeyValuePair<string, object> Info(string key, object value) {
            return new KeyValuePair<string, object>(key, value);
        }







    }

using UnityEngine;
using System.Collections;
using System;

    public class LevelChang : MonoBehaviour {

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void LevelOnChange() {
            Camera.main.transform.GetComponent<TalentTree>().currentLevel = int.Parse(this.GetComponent<UIInput>().text);
            Camera.main.transform.GetComponent<TalentTree>().ReflashNodeState();
        }

        public void UpgradeLevel() {
            this.GetComponent<UIInput>().text = (int.Parse(this.GetComponent<UIInput>().text) + 1).ToString();
        }

        public void DegradeLevel() {
            this.GetComponent<UIInput>().text = (int.Parse(this.GetComponent<UIInput>().text) - 1).ToString();
        }


    }



using UnityEngine;
using System.Collections;

public class RandomRun : MonoBehaviour {


    private float runStep = 0;
    private int runRange;

    private float startStemp;

    private Vector3 groundSize;

    public GameObject ground;

    void Start() {
        groundSize = ground.GetComponent<MeshRenderer>().bounds.size;
    }

    // Update is called once per frame
    void Update() {
        transform.Translate(Vector3.forward * Time.deltaTime * 1.5f);
        runStep = Time.time - startStemp;
        if (runStep >= runRange ||
            transform.localPosition.x <= -groundSize.x / 2 || transform.localPosition.x >= groundSize.x / 2
            || transform.localPosition.z <= -groundSize.z / 2 || transform.localPosition.z >= groundSize.z / 2) {
            startStemp = Time.time;

            if (transform.localPosition.x <= -groundSize.x / 2) transform.localPosition = new Vector3(-groundSize.x / 2, transform.localPosition.y, transform.localPosition.z);
            if (transform.localPosition.x >= groundSize.x / 2) transform.localPosition = new Vector3(groundSize.x / 2, transform.localPosition.y, transform.localPosition.z);
            if (transform.localPosition.z <= -groundSize.z / 2) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -groundSize.z / 2);
            if (transform.localPosition.z >= groundSize.z / 2) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, groundSize.z / 2);

            if (Random.Range(0, 5) == 0) {
                //Debug.Log("GoRight");
                transform.LookAt(transform.position + transform.TransformDirection(Vector3.right));
            } else {
                //Debug.Log("GoLeft");
                transform.LookAt(transform.position + transform.TransformDirection(Vector3.left));
            }
            runStep = 0;
            runRange = Random.Range(2, 10);
            //Debug.Log(runRange);
        }
    }






}

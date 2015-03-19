using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {


    public GameObject cube;
	// Use this for initialization
    private Rigidbody xxx;
	void Start () {
        //Debug.Log(Camera.main.name);
        xxx = cube.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo);
            if (hitInfo.collider != null) {
                Debug.Log(hitInfo.collider.name);
            }
        }

        xxx.AddForce(new Vector3(0f,100f,0f));

	}
}

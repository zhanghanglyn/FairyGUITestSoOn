using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimationCube : MonoBehaviour {

    public float Scale = 1.0f;
    Transform myTrans;
    Vector3 preScle = Vector3.zero;

    // Use this for initialization
    void Start () {
        myTrans = GetComponent<Transform>();
        preScle = myTrans.localScale;
    }
	
	// Update is called once per frame
	void Update () {
		if (myTrans != null )
        {
            myTrans.localScale = preScle * Scale;
        }
        

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Test());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Test()
    {
        while (true)
        {
            Debug.Log("StartTest");
            yield return new WaitForSeconds(1f);
            Debug.Log("1F");
        }
    }
}

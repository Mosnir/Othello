using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchRendering : MonoBehaviour
{

    public GameObject render3D;
    public GameObject render2D;

	// Use this for initialization
	void Start ()
    {
        render2D.SetActive(true);
        render3D.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            render2D.SetActive(!render2D.active);
            render3D.SetActive(!render3D.active);
        }
	}
}

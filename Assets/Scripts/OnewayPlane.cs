using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class OnewayPlane : MonoBehaviour {

    public DoActionsAfterXSeconds actions;
    public bool walkOver = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (walkOver == true)
        {
            Enable();
        }
	}

    void Enable()
    {
        actions.StartCountSeconds();
    }

    private void OnTriggerEnter(Collider collision)
    {
        walkOver = true;
    }
}

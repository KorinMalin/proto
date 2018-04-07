using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class PressurePlane : MonoBehaviour {
    float timeActivation = 3;
    public float currentTimeActivation = 0;
    bool done = false;
    public DoActionsAfterXSeconds actions;
    public bool walkOver = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (done)
            return;
        if (walkOver == true)
        currentTimeActivation += Time.deltaTime;


        if((currentTimeActivation) >= timeActivation)
        {
            Open();
             done = true;
        }
	}

    void Open()
    {
        actions.StartCountSeconds();
    }

    private void OnTriggerEnter(Collider collision)
    {
        walkOver = true;
    }

    private void OnTriggerExit(Collider other)
    {
        walkOver = false;
        currentTimeActivation = 0;
    }
}

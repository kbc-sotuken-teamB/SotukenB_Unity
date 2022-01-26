using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
	Animation anim;

	// Start is called before the first frame update
	void Start()
    {
		anim = this.gameObject.GetComponent<Animation>();
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKey(KeyCode.Space))
		{

			anim.Play();



		}
	}
}
/*
 
	Animation anim;

	// Use this for initialization
	void Start () {
		anim = this.gameObject.GetComponent<Animation> ();


	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {

			anim.Play ();



		}

		
	}
}
 */
using UnityEngine;
using System.Collections;

//TODO: Add camera FOV controls - limit between 30 and 80
//Put in a max/min distance/rotation for cam offsets
//Add in background image editability (global scale, position, rotation)? 

public class CamMovementScript : MonoBehaviour {

	Vector3 startPos; 
	Vector3 startRot; 

	public float stepDist = 0.25f; 
	public float rotStep = 1.0f;
	public float maxMoveDist = 20.0f; 

	public GameObject camDolly;       


	// Use this for initialization
	void Start () {
		startPos = transform.localPosition;
		startRot = transform.rotation.eulerAngles; 
	}
	
	// Update is called once per frame
	void Update () {

		//Up/down
		if(Input.GetKey(KeyCode.KeypadPlus)) 
		{
			camDolly.transform.Translate(Vector3.up * stepDist, Space.World);
		}
		if(Input.GetKey(KeyCode.KeypadMinus))   
		{
			camDolly.transform.Translate(Vector3.down * stepDist, Space.World);
		}
		
		//left/right
		if(Input.GetKey("left") || Input.GetKey("a")) 
		{
			camDolly.transform.Translate(Vector3.left * stepDist);   
		}
		if(Input.GetKey("right") || Input.GetKey("d"))
		{
			camDolly.transform.Translate(Vector3.right * stepDist);
		}
		
		//Forward/back
		if(Input.GetKey("up") || Input.GetKey("w"))           
		{
			camDolly.transform.Translate(Vector3.forward * stepDist);    
		}
		if(Input.GetKey("down") || Input.GetKey("s"))    
		{
			camDolly.transform.Translate(Vector3.back * stepDist);     
		}

		//Rotate L/R
		if(Input.GetKey("q"))          
		{   
			//transform.Rotate(0f,rotStep,0f);     
			camDolly.transform.Rotate(Vector3.up,Space.World);
		}
		if(Input.GetKey("e"))   
		{    
			camDolly.transform.Rotate(Vector3.down,Space.World);         
		}
		if(Input.GetKey("r"))   
		{   
			transform.Rotate(rotStep,0f,0f);   
		}
		if(Input.GetKey("f"))   
		{   
			transform.Rotate(-rotStep,0f,0f);     
		}
	}       
}

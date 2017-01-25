using UnityEngine;
using System.Collections;

public class AudioMeasureToNoise : MonoBehaviour {

	public float scaleMod = 1.0f;
	public float heightMod = 1.0f;

	bool modSpeed = true;
	bool modScale = true; 

	public float baseScale = 0;
	public float baseHeight = 0;
	public bool useAverageScale = true ; 
	public bool useAverageHeight = true; 

	noiseWave noiseScript;
	AudioMeasureCS audioMeasureScript;


	int frametracker = 0; 
	float avgRmsVal0 = 0;
	float avgRmsVal1 = 0;
	float avgRmsVal2 = 0;
	float avgRmsVal = 0;

	float avgDbVal0 = 0;
	float avgDbVal1 = 0;
	float avgDbVal2 = 0;
	float avgDbVal = 0;
	   
	// Use this for initialization    
	void Start () {
		//get scripts 
		noiseScript = gameObject.GetComponent<noiseWave>();
		audioMeasureScript = gameObject.GetComponent<AudioMeasureCS> ();
	}
	
	// Update is called once per frame  
	void Update () {
		
		if (frametracker > 3) 
		{
			avgDbVal0 = avgDbVal1;
			avgDbVal1 = avgDbVal2;
			avgDbVal2 = audioMeasureScript.DbValue; 
			avgDbVal = (avgDbVal0 + avgDbVal1 + avgDbVal2) / 3.0f;

			avgRmsVal0 = avgRmsVal1;
			avgRmsVal1 = avgRmsVal2;
			avgRmsVal2 = audioMeasureScript.RmsValue; 
			avgRmsVal = (avgRmsVal0 + avgRmsVal1 + avgRmsVal2) / 3.0f;
		} 
		else if (frametracker == 2)       
		{
			avgDbVal2 = audioMeasureScript.DbValue; 
			avgRmsVal2 = audioMeasureScript.RmsValue; 
		}
		else if (frametracker == 1) 
		{
			avgDbVal1 = audioMeasureScript.DbValue; 
			avgRmsVal1 = audioMeasureScript.RmsValue; 
		}
		else if (frametracker == 0)  
		{
			avgDbVal0 = audioMeasureScript.DbValue; 
			avgRmsVal0 = audioMeasureScript.RmsValue; 
		}

		frametracker++;   

		//noiseScript.perlinScale = audioMeasureScript.RmsValue * scaleMod + baseScale;
		//noiseScript.waveHeight = audioMeasureScript.DbValue * heightMod + baseHeight;
		//noiseScript.waveHeight = audioMeasureScript.PitchValue;         

		if (useAverageHeight) 
		{
			noiseScript.waveHeight = avgRmsVal * heightMod + baseHeight;                
		} 
		else 
		{
			noiseScript.waveHeight = audioMeasureScript.RmsValue  * heightMod + baseHeight;                
		}

		if (useAverageScale) 
		{
			noiseScript.perlinScale =  avgDbVal * scaleMod + baseScale; 
		} 
		else 
		{
			noiseScript.perlinScale =  audioMeasureScript.DbValue * scaleMod + baseScale;  
		}

	}
}








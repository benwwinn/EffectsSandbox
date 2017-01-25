using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class particleLines : MonoBehaviour {

	public GameObject linePrefab; 

	ParticleSystem ps; 
	ParticleSystem.Particle[] particles; 

	public List<GameObject> linesList; 

	public bool singleLine = false; 

	public int maxLines; 

	public GameObject singleLineObject;


	// Use this for initialization
	void Start () {
		ps = gameObject.GetComponent<ParticleSystem> (); 
		maxLines = ps.maxParticles;

		if (singleLine == false) 
		{
			InitializeLines (maxLines); 
		}
		else
		{
			singleLineObject = Instantiate(linePrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
			Debug.Log (singleLineObject.name);
		}
	}
	
	// Update is called once per frame
	void Update () {
		particles = new ParticleSystem.Particle[ps.particleCount];
		ps.GetParticles(particles); 

		if (singleLine) 
		{
			singleLineObject.GetComponent<LineRenderer> ().SetVertexCount (ps.particleCount);
			for (int i = 0; i < ps.particleCount; i++) 
			{
				singleLineObject.GetComponent<LineRenderer>().SetPosition(i, particles[i].position);
				//singleLineObject.GetComponent<LineRenderer> ().SetColors (ps.startColor, ps.colorOverLifetime.color.colorMax);
			}	
		}
		else
		{
			foreach(GameObject line in linesList)
			{
				line.SetActive(false);
			}

			//go through the list and find which particles to connect to 
			for(int i = 0; i < ps.particleCount; i++)    
			{
				if(i > 0)
				{
					linesList[i].SetActive(true);
					linesList[i].GetComponent<LineRenderer>().SetPosition(0, particles[i-1].position);
					linesList [i].GetComponent<LineRenderer> ().SetColors (particles [i - 1].GetCurrentColor(ps), particles [i].GetCurrentColor (ps));
					linesList[i].GetComponent<LineRenderer>().SetPosition(1, particles[i].position);  
				}
			}
		}


	}

	void InitializeLines(int maxLines)
	{
		
		linesList = new List<GameObject>();
		for(int i = 0; i < maxLines; i++)
		{
			GameObject currentLine = Instantiate(linePrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
			//add it into a list 

			linesList.Add(currentLine);
			currentLine.SetActive(false);  
		}
		
	}
}

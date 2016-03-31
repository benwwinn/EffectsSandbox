using UnityEngine;
using System.Collections;

//TOEDIT: 
//May be more beneficial to do this with game objects instead of drawn grid lines. 
//Change button press keys to UI buttons 
//Scale plane(s) along with grid
//Make plane(s) for walls/ceiling? 

//Something about this script is miscoloring the material 

public class gridOverlay : MonoBehaviour {

	public GameObject plane;
	
	public bool showMain = true;
	public bool showSub = false;
	
	public int gridSizeX;
	public int gridSizeY;
	public int gridSizeZ;
	
	public float smallStep;
	public float largeStep;
	
	public float startX;
	public float startY;
	public float startZ;
	
	public float offsetY = 0;
	public float offsetX = 0;
	public float offsetZ = 0;
	private float scrollRate = 0.1f;
	private float lastScroll = 0f;
	  
	private Material lineMaterial;

	private Color mainColor = new Color(0f,1f,0f,1f);
	private Color subColor = new Color(1f,0.5f,0f,1f);

	public GameObject floor; 
	public GameObject frontWall;
	public GameObject backWall;
	public GameObject leftWall;
	public GameObject rightWall;   


	public GameObject roomScaler;

	void Start () 
	{
	    
	}   
	
	void Update ()       
	{        
		//TODO: Add global transparency manipulator for placement/moving walls while still being able to see the background image

		if(smallStep < 1)                       
		{
			smallStep =1;       
		}                            
		if(largeStep < 1)           
		{
			largeStep =1;     
		}                     

		if(gridSizeX < 0)                 
		{
			gridSizeX = 1;      
		}
		if(gridSizeY < 0)
		{
			gridSizeY = 0;       
		}
		if(gridSizeZ < 0)
		{
			gridSizeZ = 1;
		}
		 
		//TODO: Only change these when there's been a detected change from last frame.    

		if(gridSizeY > 0)
		{
			roomScaler.transform.localScale = new Vector3 (gridSizeX/10f, gridSizeY/10f, gridSizeZ/10f);
		}
		else
		{
			roomScaler.transform.localScale = new Vector3 (gridSizeX/10f, 0.001f, gridSizeZ/10f);
		}
		//scale material with object 
		floor.GetComponent<Renderer> ().material.mainTextureScale = new Vector2 (gridSizeX / 10f, gridSizeZ / 10f);
		frontWall.GetComponent<Renderer> ().material.mainTextureScale = new Vector2 (gridSizeX / 10f, gridSizeY / 10f);
		backWall.GetComponent<Renderer> ().material.mainTextureScale = new Vector2 (gridSizeX / 10f, gridSizeY / 10f);
		leftWall.GetComponent<Renderer> ().material.mainTextureScale = new Vector2 (gridSizeZ / 10f, gridSizeY / 10f);
		rightWall.GetComponent<Renderer> ().material.mainTextureScale = new Vector2 (gridSizeZ / 10f, gridSizeY / 10f);

		//floor.transform.localScale = new Vector3 (gridSizeX/10f, gridSizeY/10f, 1.0f);
		//floorMask.transform.localScale = new Vector3 (gridSizeX/10f, gridSizeY/10f, 1.0f);
	}
	
	void CreateLineMaterial()               
	{
		
		if( !lineMaterial )                
		{      
			lineMaterial = new Material(Shader.Find("Particles/Alpha Blended"));          
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;    
			lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		}
	}
	
	void OnPostRender() 
	{        
		//may need to optomize all this later. Put it in a function that runs if it detects something changed? 

		CreateLineMaterial();   
		// set the current material
		lineMaterial.SetPass( 0 );
		
		GL.Begin( GL.LINES );

		float XAdjust = -gridSizeX / 2f + offsetX; 
		//float YAdjust = -gridSizeY / 2f + offsetY; 
		float ZAdjust = -gridSizeZ / 2f + offsetZ; 
		
		if(showSub)
		{
			GL.Color(subColor);


			//Layers
			for(float j = 0; j <= gridSizeY; j += smallStep)     
			{
					//X axis lines
					for(float i = 0; i <= gridSizeZ; i += smallStep)
					{

					if(j == gridSizeY * smallStep + offsetY || j == startY + offsetY)
					{
						//Debug.Log(gridSizeY * smallStep + offsetY);
						//Debug.Log(startY + offsetY);
						//Debug.Log( "Position Im placing line " + (j + offsetY).ToString());  
					}
				            
					if(i == gridSizeZ + offsetZ || i == startZ + offsetZ || j == gridSizeY * smallStep + offsetY || j == startY + offsetY * smallStep)
							{
								GL.Vertex3( startX +XAdjust, j + offsetY, startZ + ZAdjust + i);
								GL.Vertex3( gridSizeX +XAdjust, j + offsetY, startZ + ZAdjust + i);
							}       
						
			
					}

					//Z axis lines
					for(float i = 0; i <= gridSizeX; i += smallStep)
					{
					if(i == gridSizeX + offsetX || i == startX + offsetX || j == gridSizeY * smallStep + offsetY || j == startY + offsetY * smallStep)
							{
						GL.Vertex3( startX +XAdjust + i, j + offsetY, startZ + ZAdjust);
						GL.Vertex3( startX +XAdjust + i, j + offsetY, gridSizeZ + ZAdjust);
							}
					}
		
			}       
			  
			//Y axis lines
			for(float i = 0; i <= gridSizeZ; i += smallStep)
			{
				for(float k = 0; k <= gridSizeX; k += smallStep)
				{
					if(k == gridSizeX + offsetX || k == startX + offsetX || i == gridSizeZ + offsetZ || i == startZ + offsetZ)
					{
						GL.Vertex3( startX +XAdjust + k, startY + offsetY, startZ + ZAdjust + i);
						GL.Vertex3( startX +XAdjust + k, gridSizeY + offsetY, startZ + ZAdjust + i);
					}

				}

			} 
		}     
		
		if(showMain)     
		{
			GL.Color(mainColor);   
		             
			//Layers
			for(float j = 0; j <= gridSizeY; j += largeStep)    
			{
				//X axis lines
				for(float i = 0; i <= gridSizeZ; i += largeStep)      
				{
					
					if(j == gridSizeY * largeStep + offsetY || j == startY + offsetY)
					{
						Debug.Log(gridSizeY * largeStep + offsetY);
						Debug.Log(startY + offsetY);
						Debug.Log( "Position Im placing line " + (j + offsetY).ToString());
					}
					
					if(i == gridSizeZ + offsetZ || i == startZ + offsetZ || j == gridSizeY * largeStep + offsetY || j == startY + offsetY * largeStep)
					{
						GL.Vertex3( startX +XAdjust, j + offsetY, startZ + ZAdjust + i);
						GL.Vertex3( gridSizeX +XAdjust, j + offsetY, startZ + ZAdjust + i);
					}     
					
				}
				
				//Z axis lines
				for(float i = 0; i <= gridSizeX; i += largeStep)
				{
					if(i == gridSizeX + offsetX || i == startX + offsetX || j == gridSizeY * smallStep + offsetY || j == startY + offsetY * largeStep)
					{
						GL.Vertex3( startX +XAdjust + i, j + offsetY, startZ + ZAdjust);
						GL.Vertex3( startX +XAdjust + i, j + offsetY, gridSizeZ + ZAdjust);   
					}
				}
				        
			}        
			
			//Y axis lines
			for(float i = 0; i <= gridSizeZ; i += largeStep)      
			{
				for(float k = 0; k <= gridSizeX; k += largeStep)          
				{
					if(k == gridSizeX + offsetX || k == startX + offsetX || i == gridSizeZ + offsetZ || i == startZ + offsetZ)
					{
						GL.Vertex3( startX +XAdjust + k, startY + offsetY, startZ + ZAdjust + i);              
						GL.Vertex3( startX +XAdjust + k, gridSizeY + offsetY, startZ + ZAdjust + i);   
					}
					
				}
				
			}
		}
		
		
		GL.End();
	}
}
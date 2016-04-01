/*
The MIT License (MIT)
	
Copyright (c) 2014 <Kyle Halladay>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN            
THE SOFTWARE.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;               

public class SplinePlacer : MonoBehaviour        
{
	public Transform[] initialNodes;
	public int curveResolution;
	public bool loop;
	public GameObject objectToPlace;

	public float distanceBetweenObjects = 1.0f;                     

	public void PlaceObjects()
	{
		//To make things easier to understand
		//we're going to parse the spline into a 
		//list of Vector3s instead of using the iterator
		IEnumerable<Vector3> spline = Interpolate.NewCatmullRom(initialNodes, curveResolution, loop);
		IEnumerator iterator = spline.GetEnumerator();

		List<Vector3> splinePoints = new List<Vector3>();      
		while (iterator.MoveNext())
		{
			if (splinePoints.Count > 0)        
			{
				if ((Vector3)iterator.Current == splinePoints[0]) break;
			}
			splinePoints.Add((Vector3)iterator.Current);
		}

		//distanceToMove represents how much farther
		//we need to progress down the spline before
		//we place the next object
		int nextSplinePointIndex = 1;
		float distanceToMove = distanceBetweenObjects;

		//our current position on the spline
		Vector3 positionIterator = splinePoints[0];

		//our algo skips the first control point, so 
		//we need to manually place the first object
		GameObject.Instantiate(objectToPlace, positionIterator, Quaternion.identity);

		while(nextSplinePointIndex < splinePoints.Count)
		{
			Vector3 direction = (splinePoints[nextSplinePointIndex] - positionIterator).normalized;

			float distanceToNextPoint = Vector3.Distance(positionIterator, splinePoints[nextSplinePointIndex]);

			if (distanceToNextPoint >= distanceToMove)
			{
				positionIterator += direction*distanceToMove;
				GameObject.Instantiate(objectToPlace, positionIterator, Quaternion.identity);
				distanceToMove = distanceBetweenObjects;

			}
			else
			{
				distanceToMove -= distanceToNextPoint;
				positionIterator = splinePoints[nextSplinePointIndex++];
			}
		}
	}

	void OnDrawGizmos()
	{
		if (initialNodes == null) return;
		if (initialNodes.Length < 2) return;

		Vector3[] initialPoints = new Vector3[initialNodes.Length];

		for (int i = 0; i < initialNodes.Length; i++)
		{
			initialPoints[i] = initialNodes[i].position;
			Gizmos.DrawWireSphere(initialPoints[i], 0.1f);
		}
		
		IEnumerable<Vector3> spline = Interpolate.NewCatmullRom(initialNodes, curveResolution, loop);
		IEnumerator iterator = spline.GetEnumerator();
		
		iterator.MoveNext();
		
		Vector3 lastPoint = initialPoints[0];
		
		while (iterator.MoveNext())
		{
			Gizmos.DrawLine(lastPoint, (Vector3)iterator.Current);
			lastPoint = (Vector3)iterator.Current;
			
			//prevent an infinite loop if we want our spline to loop
			if (lastPoint == initialPoints[0]) break;
		}
	}
}

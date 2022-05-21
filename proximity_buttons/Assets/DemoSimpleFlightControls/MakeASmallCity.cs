using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker - a simple city to fly around

public class MakeASmallCity : MonoBehaviour
{
	public Material Ground;
	public Material Building;

	void Start()
	{
		var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

		float worldSize = 300.0f;

		// make some plane ground
		plane.transform.SetParent( transform);
		plane.transform.localPosition = Vector3.zero;
		plane.transform.localRotation = Quaternion.identity;
		plane.transform.localScale = Vector3.one * worldSize / 10;;

		plane.GetComponent<Renderer>().material = Ground;

		// make some building
		int count = 20;
		for (int i = 0; i <= count; i++)
		{
			float fractionX = (float)i / count;
			float fractionZ = Random.value;

			float buildingXSize = Random.Range( 5, 10);
			float buildingZSize = Random.Range( 5, 10);

			float x = Mathf.Lerp( -worldSize, +worldSize, fractionX) / 2;
			float z = Mathf.Lerp( -worldSize, +worldSize, fractionZ) / 2;

			// height
			float buildingYSize = Random.Range( 10, 45);
			float y = buildingYSize / 2;

			var cube = GameObject.CreatePrimitive( PrimitiveType.Cube);
			cube.transform.SetParent( transform);
			cube.transform.position = new Vector3( x, y, z);
			cube.transform.localScale = new Vector3( buildingXSize, buildingYSize, buildingZSize);

			// draw call? what's a draw call? :)
			var copy = new Material( Building);
			copy.mainTextureScale = new Vector3( buildingXSize, buildingYSize) / 3;

			cube.GetComponent<Renderer>().material = copy;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFloodfill : MonoBehaviour
{
	public Texture2D sourceTexture;

	public Material material;

	GameObject quad;

	Texture2D workTexture;

	IEnumerator Start ()
	{
		material = new Material( material);

		quad = GameObject.CreatePrimitive( PrimitiveType.Quad);
		quad.GetComponent<Renderer>().material = material;

		workTexture = Instantiate<Texture2D>( sourceTexture);

		yield return new WaitUntil( () => {
			return Input.GetKeyDown( KeyCode.Space);
		});

		ImageUtils.FloodFill( sourceTexture, workTexture, Color.red, Color.black, 10, 10);

		material.mainTexture = workTexture;
		quad.GetComponent<Renderer>().material = material;
	}
}

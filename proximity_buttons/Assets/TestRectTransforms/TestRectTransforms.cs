using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRectTransforms : MonoBehaviour
{
	public RectTransform rtThingy;

	string RTF( RectTransform rt)
	{
		return System.String.Format( "RT.R: ({0},{1},{2},{3}) -> ({4},{5})",
			(int)rt.rect.x,
			(int)rt.rect.y,
			(int)rt.rect.width,
			(int)rt.rect.height,
			(int)rt.position.x,
			(int)rt.position.y);
	}

	string VF( Vector3 v)
	{
		return System.String.Format( "V3: ({0},{1},{2})", (int)v.x, (int)v.y, (int)v.z);
	}

	Vector2 xxx;

	Vector3[] four = new Vector3[4];

	void Update ()
	{
		RectTransform rtCanvas = rtThingy.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

		string s = RTF( rtThingy) + "\n" +
			RTF( rtCanvas) + "\n";

		rtThingy.GetWorldCorners( four);

		s += VF( four[0]) + "\n";
		s += VF( four[1]) + "\n";
		s += VF( four[2]) + "\n";
		s += VF( four[3]) + "\n";

		Vector3 pos = Input.mousePosition;

		pos.x /= Screen.width;
		pos.y /= Screen.height;

		pos.x *= rtCanvas.rect.width;
		pos.y *= rtCanvas.rect.height;

		s += System.String.Format( "\nW: ({0},{1})", (int)pos.x, (int)pos.y);

		pos.x -= rtThingy.position.x;
		pos.y -= rtThingy.position.y;

		s += System.String.Format( "\nL: ({0},{1})", (int)pos.x, (int)pos.y);

		if (pos.x >= rtThingy.rect.x)
		{
			if (pos.x < rtThingy.rect.x + rtThingy.rect.width)
			{
				if (pos.y >= rtThingy.rect.y)
				{
					if (pos.y < rtThingy.rect.y + rtThingy.rect.height)
					{
						s += "         - IN!";
					}
				}
			}
		}

		DSM.Output.Value = s;
	}
}

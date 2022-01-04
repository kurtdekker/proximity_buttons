using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Collections/GameObjectCollection")]
public class GameObjectCollection : ScriptableObject
{
	public string Description;

	public GameObject[] GameObjects;

	public GameObject this[int index]
	{
		get
		{
			return GameObjects[index];
		}
		set
		{
			GameObjects[index] = value;
		}
	}

	public	float	MasterScaling = 1.0f;

	public void InstantiateAll( Transform parent = null)
	{
		foreach( var go in GameObjects)
		{
			GameObject go2 = Instantiate<GameObject>( go);
			go2.transform.SetParent( parent);
		}
	}

	public GameObject PickRandom()
	{
		return GameObjects[ Random.Range ( 0, GameObjects.Length)];
	}

	int DeckPointer;
	GameObject[] GameObjectDeck;

	void TryShuffle()
	{
		if (GameObjectDeck == null ||
			(GameObjectDeck.Length != GameObjects.Length) ||
			DeckPointer >= GameObjectDeck.Length)
		{
			DeckPointer = 0;

			GameObjectDeck = new GameObject[ GameObjects.Length];

			for (int i = 0; i < GameObjects.Length; i++)
			{
				GameObjectDeck[i] = GameObjects[i];
			}

			for (int i = 0; i < GameObjectDeck.Length - 1; i++)
			{
				int j = Random.Range ( i, GameObjectDeck.Length);

				var t = GameObjectDeck[i];
				GameObjectDeck[i] = GameObjectDeck[j];
				GameObjectDeck[j] = t;
			}
		}
	}

	public GameObject PickNextShuffled()
	{
		TryShuffle ();

		GameObject result = GameObjectDeck [DeckPointer];

		DeckPointer++;

		return result;
	}

	public int Length
	{
		get
		{
			if (GameObjects == null)
			{
				return 0;
			}
			return GameObjects.Length;
		}
	}

	public int ConstrainIndex( int index, bool wrap = false)
	{
		if (GameObjects.Length < 1)
		{
			throw new System.IndexOutOfRangeException("GameObjectCollection.ConstrainIndex(): no GameObjects in " + name);
		}

		if (index < 0) return 0;
		if (index >= GameObjects.Length)
		{
			if (wrap)
			{
				index = 0;
			}
			else
			{
				index = GameObjects.Length - 1;
			}
		}
		return index;
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(GameObjectCollection))]
	public class GameObjectCollectionInspector : Editor
	{
		Transform parent;

		void MakeContents()
		{
			GameObjectCollection goc = (GameObjectCollection)this.target;

			parent = new GameObject( "GOC:" + goc.name).transform;

			float spacing = 5.0f;
			if (goc.MasterScaling >= 1.5f)
			{
				spacing = goc.MasterScaling * 1.1f;
			}

			for (int i = 0; i < goc.GameObjects.Length; i++)
			{
				var go = goc.GameObjects[i];

				if (go)
				{
					var go2 = (GameObject)PrefabUtility.InstantiatePrefab(go);
					if (go2)
					{
						go2.transform.SetParent( parent);

						go2.transform.localScale = Vector3.one * goc.MasterScaling;

						Vector3 pos = Vector3.right * (i - (goc.GameObjects.Length - 1) * 0.5f) * spacing;
						go2.transform.localPosition = pos;
					}
				}
			}
		}

		public override void OnInspectorGUI()
		{
			this.DrawDefaultInspector ();

			GUILayout.Space (25);

			GUILayout.BeginHorizontal();
			if (parent)
			{
				GUI.color = new Color( 1.0f, 0.5f, 0.5f);
				if(GUILayout.Button("Clear Instantiated Prefabs"))
				{
					DestroyImmediate( parent.gameObject);
					parent = null;
				}
			}
			else
			{
				GUI.color = new Color( 0.5f, 1.0f, 0.5f);
				if(GUILayout.Button("Instantiate Prefabs Above"))
				{
					MakeContents();
				}
			}
			GUILayout.EndHorizontal();
		}
	}
#endif
}

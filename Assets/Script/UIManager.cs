using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public static UIManager sUIManager;

	private Dictionary<string, GameObject> mUIPrefabs;

	private void Awake()
	{
		sUIManager = this;
	}

	// Use this for initialization
	void Start ()
	{
		mUIPrefabs = new Dictionary<string, GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	/// <summary>
	/// 返回UI的预制
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public GameObject GetUIPrefab(string name)
	{
		GameObject uiPrefab;
		mUIPrefabs.TryGetValue(name, out uiPrefab);
		if (uiPrefab == null)
		{
			uiPrefab = (GameObject)Resources.Load(name);
			if (uiPrefab != null)
				mUIPrefabs.Add(name, uiPrefab);
		}

		return uiPrefab;
	}

	/// <summary>
	/// 实例化一个UI
	/// </summary>
	/// <param name="name"></param>
	/// <param name="position"></param>
	/// <param name="rotate"></param>
	/// <returns></returns>
	public GameObject GetUiInstance(string name, Vector3 position, Quaternion rotate)
	{
		GameObject uiPrefab;
		mUIPrefabs.TryGetValue(name, out uiPrefab);
		if (uiPrefab == null)
		{
			uiPrefab = (GameObject)Resources.Load(name);
			if (uiPrefab != null)
				mUIPrefabs.Add(name, uiPrefab);
		}

		if (mUIPrefabs == null)
			return null;

		return Instantiate(uiPrefab, position, rotate);
	}
}

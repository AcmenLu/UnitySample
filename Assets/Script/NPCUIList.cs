using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCUIList : MonoBehaviour
{
	public static NPCUIList sNPCUIList;

	Dictionary<GameObject, GameObject> mNPCItems;

	private void Awake()
	{
		sNPCUIList = this;
		mNPCItems = new Dictionary<GameObject, GameObject>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	/// <summary>
	/// 添加一个子项
	/// </summary>
	/// <param name="item"></param>
	/// <param name="regObject"></param>
	/// <returns></returns>
	public GameObject AddChildItem(GameObject item, GameObject regObject)
	{
		float posy = -72 * transform.childCount;
		GameObject go = NGUITools.AddChild(gameObject, item);
		go.transform.localPosition = new Vector3(0, posy, 0);
		if (regObject != null)
			mNPCItems.Add(regObject, go);

		return go;
	}

	/// <summary>
	/// 通过引用对象获取UI
	/// </summary>
	/// <param name="regObject"></param>
	/// <returns></returns>
	public GameObject GetChildByRef(GameObject regObject)
	{
		GameObject outobj = null;
		mNPCItems.TryGetValue(regObject, out outobj);
		return outobj;
	}

	/// <summary>
	/// 根据引用删除一个子UI
	/// </summary>
	/// <param name="regObject"></param>
	public void DestoryChildByRef(GameObject regObject)
	{
		GameObject outobj = null;
		mNPCItems.TryGetValue(regObject, out outobj);
		if(outobj != null)
		{
			mNPCItems.Remove(regObject);
			Destroy(outobj);
		}
	}

	/// <summary>
	/// 根据UI获取引用对象
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	public GameObject GetRefByUIObj(GameObject obj)
	{
		GameObject outobj = null;
		foreach (GameObject key in mNPCItems.Keys)
		{
			if(mNPCItems[key].Equals(obj))
			{
				outobj = key;
				break;
			}
		}
		return outobj;
	}
}

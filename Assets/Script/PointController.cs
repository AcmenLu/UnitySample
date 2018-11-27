using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour
{
	private bool mHasMonster = false;
	private string mPrefabPath;

	public bool HasMonster
	{
		get { return mHasMonster; }
		set { mHasMonster = value; }
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (mHasMonster == true)
			return;

		if (GameManager.sGameManager == null || GameManager.sGameManager.Player == null)
			return;

		float playerDis = (transform.position - GameManager.sGameManager.Player.transform.position).magnitude;
		if (playerDis < 10)
		{
			GameObject monster = MonsterManager.sMonsterManager.CreateMonster(mPrefabPath, transform.position);
			mHasMonster = true;
			monster.GetComponent<DragonController>().Point = this;
		}
	}

	/// <summary>
	/// 设置要创建的怪物的路径
	/// </summary>
	/// <param name="path"></param>
	public void SetPrefabPath(string path)
	{
		mPrefabPath = path;
	}

}

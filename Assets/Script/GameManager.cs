using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static List<GameObject> sMonsterLst = new List<GameObject>();
	public static GameObject sPlayer = null;
	public static GameManager sGameManager;
	public static Vector3 sPlayerPos = new Vector3(100, 1, 100);


	// 怪物位置的列表
	private Vector3[] mMonsPos = {
		new Vector3(100, 1, 80),
		//new Vector3(80, 1, 80),
		//new Vector3(80, 1, 100),
	};

	private GameObject mMonsterPrefab;
	private GameObject mPlayerPrefab;

	// Use this for initialization
	void Start ()
	{
		sGameManager = this;
		mMonsterPrefab = (GameObject)Resources.Load("Prefabs/DragonBoss");
		mPlayerPrefab = (GameObject)Resources.Load("Prefabs/Player");

		if (mMonsterPrefab != null)
			InitMonster();

		if (mPlayerPrefab != null)
			CreatePlayer(2);
	}

	void Update()
	{

	}

	/// <summary>
	/// 初始化所有的怪物
	/// </summary>
	void InitMonster()
	{
		for (int i = 0; i < mMonsPos.Length; i++)
			CreateDragMonster(mMonsPos[i]);
	}

	/// <summary>
	/// 创建一个主角在场景中
	/// </summary>
	void InitPlayer()
	{
		sPlayer = Instantiate(mPlayerPrefab, sPlayerPos, Quaternion.identity);
	}

	/// <summary>
	/// 延迟创建一个主角
	/// </summary>
	/// <param name="position"></param>
	void CreatePlayer(float dealy = 0)
	{
		if (dealy <= 0)
			InitPlayer();
		else
			Invoke("InitPlayer", dealy);
	}

	/// <summary>
	/// 创建一个怪物
	/// </summary>
	/// <param name="position"></param>
	GameObject CreateDragMonster(Vector3 position)
	{
		GameObject monster = Instantiate(mMonsterPrefab, position, Quaternion.identity);
		sMonsterLst.Add(monster);
		return monster;
	}

	/// <summary>
	/// 销毁一个怪物
	/// </summary>
	/// <param name="time"></param>
	/// <param name="monster"></param>
	public static void DestoryMonster(float time, GameObject monster)
	{
		if (monster == null || sMonsterLst.Count <= 0)
			return;

		if (sMonsterLst.Contains(monster))
		{
			sMonsterLst.Remove(monster);
			Destroy(monster, time);
		}
	}

	/// <summary>
	/// 主角死亡
	/// </summary>
	/// <param name="time"></param>
	public static void DestoryPlayer(float time)
	{
		Destroy(sPlayer, time);
		sGameManager.CreatePlayer(time + 2);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
	public static MonsterManager sMonsterManager;

	// 怪物位置的列表
	private Vector3[] mMonsPos = {
		new Vector3(140, 1, 210),
		new Vector3(67, 1, 240),
		new Vector3(113, 1, 260),

		new Vector3(260, 1, 250),
		new Vector3(240, 1, 190),
		new Vector3(190, 1, 220),

		new Vector3(170, 1, 75),
		new Vector3(90, 1, 115),
		new Vector3(120, 1, 150),
	};

	private GameObject mPointPrefab;
	private List<GameObject> mMonsterLst;
	private Dictionary<string, GameObject> mMonsterPrefabs;

	public List<GameObject> MonsterLst
	{
		get { return mMonsterLst; }
	}

	private void Awake()
	{
		sMonsterManager = this;
		mMonsterPrefabs = new Dictionary<string, GameObject>();
		mMonsterLst = new List<GameObject>();
	}

	// Use this for initialization
	void Start ()
	{
		mPointPrefab = (GameObject)Resources.Load("Prefabs/EmptyPoint");
		if (mPointPrefab != null)
			InitMonsterPoint();
	}

	/// <summary>
	/// 初始化所有的怪物
	/// </summary>
	void InitMonsterPoint()
	{
		for (int i = 0; i < mMonsPos.Length; i++)
		{
			GameObject point = Instantiate(mPointPrefab, mMonsPos[i], Quaternion.identity);
			point.GetComponent<PointController>().SetPrefabPath("Prefabs/DragonBoss");
		}
	}

	/// <summary>
	/// 创建一个怪物
	/// </summary>
	/// <param name="position"></param>
	public GameObject CreateMonster(string name, Vector3 position)
	{
		GameObject prefab;
		mMonsterPrefabs.TryGetValue(name, out prefab);

		if (prefab == null)
		{
			prefab = (GameObject)Resources.Load(name);
			if (prefab != null)
				mMonsterPrefabs.Add(name, prefab);
		}

		if (prefab == null)
			return null;

		GameObject monster =  Instantiate(prefab, position, Quaternion.identity);
		mMonsterLst.Add(monster);
		return monster;
	}

	/// <summary>
	/// 销毁一个怪物
	/// </summary>
	/// <param name="time"></param>
	/// <param name="monster"></param>
	public void DestoryMonster(float time, GameObject monster)
	{
		if (monster == null || mMonsterLst.Count <= 0)
			return;

		if (mMonsterLst.Contains(monster))
		{
			mMonsterLst.Remove(monster);
			Destroy(monster, time);
		}
	}
}

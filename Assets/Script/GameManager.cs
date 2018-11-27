using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager sGameManager = null;

	private GameObject mPlayerPrefab;
	private Vector3 mPlayerPos;
	private GameObject mPlayer = null;

	public Vector3 PlayerPosition
	{
		get { return mPlayerPos; }
		set { mPlayerPos = value; }
	}

	public GameObject Player
	{
		get { return mPlayer; }
	}


	private void Awake()
	{
		sGameManager = this;
	}

	// Use this for initialization
	void Start ()
	{
		mPlayerPrefab = (GameObject)Resources.Load("Prefabs/Player");
		mPlayerPos = new Vector3(100, 1, 100);

		if (mPlayerPrefab != null)
			CreatePlayer(1);
	}

	void Update()
	{
	}

	/// <summary>
	/// 创建一个主角在场景中
	/// </summary>
	void InitPlayer()
	{
		mPlayer = Instantiate(mPlayerPrefab, mPlayerPos, Quaternion.identity);
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
	/// 主角死亡
	/// </summary>
	/// <param name="time"></param>
	public void DestoryPlayer(float time)
	{
		Destroy(mPlayer, time);
		sGameManager.CreatePlayer(time + 2);
	}
}

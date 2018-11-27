using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatricleManager : MonoBehaviour
{
	public static PatricleManager sController;

	Dictionary<string, GameObject> mParticlePrefabs;

	private void Awake()
	{
		sController = this;
	}

	// Use this for initialization
	void Start ()
	{
		mParticlePrefabs = new Dictionary<string, GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	/// <summary>
	/// 获取一个特效播放的实体
	/// </summary>
	/// <param name="name"></param>
	/// <param name="position"></param>
	/// <returns></returns>
	public GameObject GetParticlePre(string name, Vector3 position, Quaternion rotate)
	{
		GameObject prefab;
		mParticlePrefabs.TryGetValue(name, out prefab);
		if (prefab == null)
		{
			prefab = (GameObject)Resources.Load(name);
			if (prefab != null)
				mParticlePrefabs.Add(name, prefab);
		}

		if (prefab == null)
			return null;

		return Instantiate(prefab, position, rotate);
	}

	/// <summary>
	/// 播放特效
	/// </summary>
	/// <param name="particle"></param>
	public void PlayePatricle(ParticleSystem particle)
	{
		if (particle != null)
			particle.Play();
	}

	/// <summary>
	/// 停止特效播放
	/// </summary>
	/// <param name="particle"></param>
	public void StopPatricle(ParticleSystem particle)
	{
		if (particle != null)
			particle.Stop();
	}
}

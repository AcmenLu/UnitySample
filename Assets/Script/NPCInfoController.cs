using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInfoController : MonoBehaviour
{
	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update()
	{

	}

	/// <summary>
	/// 设置所有信息
	/// </summary>
	/// <param name="name"></param>
	/// <param name="icon"></param>
	/// <param name="hp"></param>
	private void InitInfo(string name, string icon, float hp)
	{
		SetName(name);
		SetIcon(icon);
		SetHP(hp);
	}

	/// <summary>
	/// 设置名称
	/// </summary>
	/// <param name="name"></param>
	public void SetName(string name)
	{
		Transform namelabel = transform.Find("Label_name");
		if (namelabel != null)
			namelabel.GetComponent<UILabel>().text = name;
	}

	/// <summary>
	/// 设置图片
	/// </summary>
	public void SetIcon(string icon)
	{
		Transform namelabel = transform.Find("Sprite_icon");
		if (namelabel != null)
			namelabel.GetComponent<UISprite>().spriteName = icon;
	}

	/// <summary>
	/// 点击事件
	/// </summary>
	public void OnItemClick()
	{
		if (GameManager.sGameManager == null)
			return;

		if (GameManager.sGameManager.Player == null)
			return;

		PlayerController player = GameManager.sGameManager.Player.GetComponent<PlayerController>();
		if (player == null)
			return;

		GameObject target = NPCUIList.sNPCUIList.GetRefByUIObj(this.gameObject);
		if (target != null)
			player.SetFindTarget(target);
		//player.FindTarget = mTarget;
	}

	/// <summary>
	/// 设置生命值
	/// </summary>
	public void SetHP(float hp)
	{
		Transform namelabel = transform.Find("Label_hp");
		if (namelabel != null)
			namelabel.GetComponent<UILabel>().text = hp.ToString();
	}

	/// <summary>
	/// 全局创建一个血条
	/// </summary>
	/// <returns></returns>
	public static GameObject CreateInfoItem(string name, string icon, float hp, GameObject target)
	{
		GameObject obj = UIManager.sUIManager.GetUIPrefab("UI/Prefab/NPCItem");
		if (obj == null)
			return null;

		GameObject item =  NPCUIList.sNPCUIList.AddChildItem(obj, target);
		if (item == null)
			return null;

		NPCInfoController info = obj.GetComponent<NPCInfoController>();
		if (info == null)
			return null;

		info.InitInfo(name, icon, hp);
		return item;
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodProgress : MonoBehaviour
{
	private Camera mTargetCamera;
	private Camera mGUICamera;
	private GameObject mFollowTarget;
	private UISlider mSlider;
	private UILabel mText;
	private Vector3 mFollowOffset;

	private void Awake()
	{
		mSlider = gameObject.GetComponent<UISlider>();
		mText = transform.Find("ValueLabel").GetComponent<UILabel>();
		mText.text = "";
		mFollowOffset = new Vector3();
	}

	// Use this for initialization
	void Start ()
	{
		mGUICamera = NGUITools.FindCameraForLayer(this.gameObject.layer);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	private void LateUpdate()
	{
		if (mFollowTarget == null)
			return;

		Vector3 targetPos = mFollowTarget.transform.position + mFollowOffset;
		Vector3 screepos = mTargetCamera.WorldToScreenPoint(targetPos);
		Vector3 result = mGUICamera.ScreenToWorldPoint(screepos);
		result.z = 0;
		transform.position = result;
	}

	/// <summary>
	/// 设置跟随目标
	/// </summary>
	/// <param name="target"></param>
	public void SetTarget(GameObject target)
	{
		if (target == null)
			return;

		mFollowTarget = target;
		mTargetCamera = NGUITools.FindCameraForLayer(mFollowTarget.gameObject.layer);
	}

	/// <summary>
	/// 设置跟随偏移
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	public void SetFollowOffset(float x, float y, float z)
	{
		mFollowOffset.x = x;
		mFollowOffset.y = y;
		mFollowOffset.z = z;
	}
	
	/// <summary>
	/// 设置跟随目标偏移
	/// </summary>
	/// <param name="offset"></param>
	public void SetFollowOffset(Vector3 offset)
	{
		SetFollowOffset(offset.x, offset.y, offset.z);
	}

	/// <summary>
	/// 设置进度
	/// </summary>
	/// <param name="value"></param>
	public void SetValue(float value)
	{
		if (mSlider == null)
			return;

		mSlider.value = Mathf.Max(value, 0);
	}

	/// <summary>
	/// 设置显示的文本
	/// </summary>
	/// <param name="text"></param>
	public void SetText(string text)
	{
		if (mText == null)
			return;
		mText.text = text;
	}

	/// <summary>
	/// 全局创建一个血条
	/// </summary>
	/// <returns></returns>
	public static BloodProgress CreateBloodProgress(GameObject target, Vector3 offset)
	{
		GameObject obj = UIManager.sUIManager.GetUiInstance("UI/Prefab/ProgressBar", target.transform.position + offset, Quaternion.identity);
		if (obj == null)
			return null;

		BloodProgress bar = obj.GetComponent<BloodProgress>();
		if (bar == null)
			return null;

		bar.SetTarget(target);
		bar.SetFollowOffset(offset);
		return bar;
	}

	/// <summary>
	/// 销毁一个血条
	/// </summary>
	/// <param name="progress"></param>
	public static void DestoryBloodProgress(BloodProgress progress)
	{
		Destroy(progress.gameObject);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private enum AttackState
	{
		ATTACK_1,
		ATTACK_2,
		ATTACK_3,
	}

	private Animation mAnimation;
	private float mMoveSpeed = 450f;
	private CharacterController mCharacter;
	private float mAniRetainTime;
	private Vector3 mLookAt;
	private float mAttackLen = 7;
	private float mHp = 400;
	private bool mDeath = false;

	// Use this for initialization
	void Start()
	{
		mCharacter = gameObject.GetComponent<CharacterController>();
		mAnimation = gameObject.GetComponent<Animation>();
		mAnimation.Play("ZhanLi_TY");
	}

	// Update is called once per frame
	void Update()
	{
		if (mDeath == true)
			return;

		mAniRetainTime -= Time.deltaTime;
		if (Input.GetKey(KeyCode.W))
		{
			mLookAt = Vector3.forward;
			Run();
		}
		else if (Input.GetKey(KeyCode.S))
		{
			mLookAt = Vector3.back;
			Run();
		}
		else if (Input.GetKey(KeyCode.A))
		{
			mLookAt = Vector3.left;
			Run();
		}
		else if(Input.GetKey(KeyCode.D))
		{
			mLookAt = Vector3.right;
			Run();
		}
		else if (Input.GetKey(KeyCode.Q))
		{
			Attack(AttackState.ATTACK_1);
		}
		else if (Input.GetKey(KeyCode.E))
		{
			Attack(AttackState.ATTACK_2);
		}
		else if (Input.GetKey(KeyCode.R))
		{
			Attack(AttackState.ATTACK_3);
		}
		else
		{
			if (mAniRetainTime <= 0)
			{
				mAnimation.Play("ZhanLi_TY");
				mAniRetainTime = 0;
			}
		}
	}

	/// <summary>
	/// 跑
	/// </summary>
	void Run()
	{
		if (mAniRetainTime > 0)
			return;

		transform.rotation = Quaternion.LookRotation(mLookAt.normalized, Vector3.up);
		mCharacter.SimpleMove(transform.forward * Time.deltaTime * mMoveSpeed);
		mAnimation.Play("BenPao_TY");
		mAniRetainTime = 0f;
	}

	/// <summary>
	/// 攻击
	/// </summary>
	void Attack(AttackState state)
	{
		if (mAniRetainTime > 0)
			return;

		if (state == AttackState.ATTACK_1)
		{
			mAnimation.Play("PuTongGongJi1_JJ");
			mAniRetainTime = mAnimation["PuTongGongJi1_JJ"].length;
			AttackMonster(10);
		}
		else if (state == AttackState.ATTACK_2)
		{
			mAnimation.Play("PuTongGongJi2_JJ");
			AttackMonster(55);
			mAniRetainTime = mAnimation["PuTongGongJi2_JJ"].length;
		}
		else if (state == AttackState.ATTACK_3)
		{
			mAnimation.Play("PuTongGongJi3_JJ");
			AttackMonster(55);
			mAniRetainTime = mAnimation["PuTongGongJi3_JJ"].length;
		}
	}

	/// <summary>
	/// 判断是否攻击到了怪物
	/// </summary>
	/// <param name="hurt"></param>
	void AttackMonster(float hurt)
	{
		for(int i = 0; i < GameManager.sMonsterLst.Count; i ++)
		{
			GameObject monster = GameManager.sMonsterLst[i];
			DragonController ctr = monster.GetComponent<DragonController>();
			if (ctr.IsDeath())
				continue;

			Vector3 dir = monster.transform.position - transform.position;
			if (dir.magnitude > mAttackLen)
				continue;

			float cos = Vector3.Dot(dir.normalized, transform.forward);
			if (cos <= 0.2f)
				continue;

			ctr.GetHit(hurt);
		}
	}

	/// <summary>
	/// 主角受到攻击
	/// </summary>
	/// <param name="hurt"></param>
	public void GetHit(float hurt)
	{
		mHp -= hurt;
		if (mHp <= 0)
		{
			mDeath = true;
			mAnimation.Play("SiWang_JJ");
			GameManager.DestoryPlayer(mAnimation["SiWang_JJ"].length);
			GameManager.sPlayerPos = transform.position;
		}
	}

	/// <summary>
	/// 判断是否死亡
	/// </summary>
	/// <returns></returns>
	public bool IsDeath()
	{
		return mDeath;
	}
}
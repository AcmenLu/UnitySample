using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{

	private enum MonsterState
	{
		STAND,		// 原地呼吸
		WALK,		// 移动
		CHASE,		// 追击玩家
		RETURN,		// 超出追击范围后返回
		ATTACK,		// 攻击
	}

	private Animation mAnimation;
	private CharacterController mCharacter;

	private Vector3 mStartPosition;

	private float mWanderRadius; // 游走半径
	private float mChaseRadius; // 追击半径
	private float mAttackRadius; // 攻击半径
	private float mMaxDistance; //离开家最远距离

	private float mResetTime; // 状态之间切换的时间
	private float mLastChangeTime; // 记录上一次自动切换状态时间
	private MonsterState mState;
	private Quaternion mTargetRotation;
	private float mRetainTime = 0;

	private float mMoveSpeed;

	private float mHP = 200f;
	private bool mDeath = false;

	// Use this for initialization
	void Start ()
	{
		mAnimation = gameObject.GetComponent<Animation>();
		mCharacter = gameObject.GetComponent<CharacterController>();
		mStartPosition = transform.position;

		mWanderRadius = 15f;
		mChaseRadius = 20f;
		mAttackRadius = 7f;
		mMaxDistance = 30f;
		mResetTime = 2f;

		mMoveSpeed = 300f;
		mState = MonsterState.WALK;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (mDeath == true)
			return;

		switch (mState)
		{
			case MonsterState.STAND:
				if (Time.time - mLastChangeTime > mResetTime)
					RandomAction();

				mAnimation.Play("PuTongGongJi_1");
				DiatanceCheck();
				break;

			case MonsterState.WALK:
				transform.rotation = Quaternion.Slerp(transform.rotation, mTargetRotation, 0.1f);
				MoveForward();
				if (Time.time - mLastChangeTime > mResetTime)
					RandomAction();

				mAnimation.Play("BenPao");
				WanderRadiusCheck();
				break;

			case MonsterState.CHASE:
				if (GameManager.sPlayer == null)
				{
					mState = MonsterState.RETURN;
					break;
				}
				transform.rotation = Quaternion.LookRotation(GameManager.sPlayer.transform.position - transform.position, Vector3.up);
				MoveForward();
				mAnimation.Play("BenPao");
				ChaseRadiusCheck();
				break;

			case MonsterState.RETURN:
				transform.rotation = Quaternion.LookRotation(mStartPosition - transform.position, Vector3.up);
				MoveForward();
				mAnimation.Play("BenPao");
				ReturnRadiusCheck();
				break;

			case MonsterState.ATTACK:
				if (GameManager.sPlayer == null)
				{
					mState = MonsterState.RETURN;
					break;
				}
				transform.rotation = Quaternion.LookRotation(GameManager.sPlayer.transform.position - transform.position, Vector3.up);
				Attack();
				CheckAttack();
				break;
		}
	}

	/// <summary>
	/// 向前移动
	/// </summary>
	void MoveForward()
	{
		mCharacter.SimpleMove(transform.forward * Time.deltaTime * mMoveSpeed);
	}
	/// <summary>
	/// 随机设置一个状态
	/// </summary>
	void RandomAction()
	{
		mLastChangeTime = Time.time;
		float rate = Random.Range(0f, 3f);
		if (rate <= 2)
		{
			mState = MonsterState.STAND;
		}
		else
		{
			mState = MonsterState.WALK;
			mTargetRotation = Quaternion.Euler(0, Random.Range(0.5f, 2) * 90, 0);
		}
	}

	/// <summary>
	/// 检查主角是否有效
	/// </summary>
	/// <returns></returns>
	bool CheckPlayer()
	{
		if (GameManager.sPlayer == null)
			return false;

		return GameManager.sPlayer.GetComponent<PlayerController>().IsDeath() == false;
	}
	/// <summary>
	/// 普通状态下的距离检测
	/// </summary>
	void DiatanceCheck()
	{
		if (CheckPlayer() == false)
			return;

		float diatanceToPlayer = Vector3.Distance(GameManager.sPlayer.transform.position, transform.position);
		if (diatanceToPlayer < mAttackRadius)
			mState = MonsterState.ATTACK;
		else if (diatanceToPlayer < mChaseRadius)
			mState = MonsterState.CHASE;
	}

	/// <summary>
	/// 游走的距离检测
	/// </summary>
	void WanderRadiusCheck()
	{
		float homeDiatance = Vector3.Distance(mStartPosition, transform.position);
		if (homeDiatance > mWanderRadius)
			mTargetRotation = Quaternion.LookRotation(mStartPosition - transform.position, Vector3.up);

		if (CheckPlayer() == false)
			return;

		float diatanceToPlayer = Vector3.Distance(GameManager.sPlayer.transform.position, transform.position);
		if (diatanceToPlayer < mAttackRadius)
			mState = MonsterState.ATTACK;
		else if (diatanceToPlayer < mChaseRadius)
			mState = MonsterState.CHASE;
	}

	/// <summary>
	/// 追击距离检测
	/// </summary>
	void ChaseRadiusCheck()
	{
		float homeDiatance = Vector3.Distance(mStartPosition, transform.position);
		if (homeDiatance > mMaxDistance)
		{
			mState = MonsterState.RETURN;
			return;
		}

		if (CheckPlayer() == false)
			return;

		float diatanceToPlayer = Vector3.Distance(GameManager.sPlayer.transform.position, transform.position);
		if (diatanceToPlayer < mAttackRadius)
			mState = MonsterState.ATTACK;
		else if (diatanceToPlayer > mChaseRadius)
			mState = MonsterState.RETURN;
	}

	/// <summary>
	/// 追击返回函数
	/// </summary>
	void ReturnRadiusCheck()
	{
		float homeDiatance = Vector3.Distance(mStartPosition, transform.position);
		if (homeDiatance < mWanderRadius)
			RandomAction();
	}
	
	/// <summary>
	/// 攻击判断
	/// </summary>
	void CheckAttack()
	{
		if (CheckPlayer() == false)
			return;

		float diatanceToPlayer = Vector3.Distance(GameManager.sPlayer.transform.position, transform.position);
		if (diatanceToPlayer > mAttackRadius)
		{
			float homeDiatance = Vector3.Distance(mStartPosition, transform.position);
			if (homeDiatance > mMaxDistance)
			{
				mState = MonsterState.RETURN;
			}
			else
			{
				if (diatanceToPlayer > mChaseRadius)
				{
					if (homeDiatance < mWanderRadius)
						RandomAction();
					else
						mState = MonsterState.RETURN;
				}
				else
				{
					mState = MonsterState.CHASE;
				}
			}
		}

	}

	/// <summary>
	/// 攻击函数
	/// </summary>
	void Attack()
	{
		if (CheckPlayer() == false)
		{
			mState = MonsterState.RETURN;
			return;
		}

		mRetainTime -= Time.deltaTime;
		if (mRetainTime > 0)
			return;
	
		mAnimation.Play("PuTongGongJi_2");
		mRetainTime = mAnimation["PuTongGongJi_2"].length + 0.5f;
		GameManager.sPlayer.GetComponent<PlayerController>().GetHit(100);
	}

	/// <summary>
	/// 受到攻击
	/// </summary>
	/// <param name="hurt"></param>
	public void GetHit(float hurt)
	{
		mHP -= hurt;
		if (mHP <= 0)
		{
			mDeath = true;
			mAnimation.Play("SiWang");
			GameManager.DestoryMonster(mAnimation["SiWang"].length, gameObject);
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

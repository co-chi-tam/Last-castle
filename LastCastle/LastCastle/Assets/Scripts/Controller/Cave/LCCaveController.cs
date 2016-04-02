using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LCCaveController : LCBaseController
{
	#region Properties

	#endregion

	#region Implementation Monobehaviour

	public override void Init()
	{
		base.Init();

		var idle = new FSMCastleIdleState(this);
		var waiting = new FSMCastleWaitingState(this);
		var spawnMember = new FSMCastleSpawnTroopState(this);
		var spawnAllMember = new FSMCastleSpawnAllTroopState(this);

		m_FSMManager.RegisterState("CaveIdleState", idle);
		m_FSMManager.RegisterState("CaveWaitingState", waiting);
		m_FSMManager.RegisterState("CaveSpawnMemberState", spawnMember);
		m_FSMManager.RegisterState("CaveSpawnAllMemberState", spawnAllMember);

		m_FSMManager.RegisterCondition("IsFullCave", IsFullCave);

		m_FSMManager.LoadFSM(m_Entity.GetFSMPath());
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		m_Entity.Update(Time.fixedDeltaTime);
		m_FSMManager.UpdateState();
	}

	#endregion

	#region Main Methods

	public override Dictionary<string, object> GetObjectCurrentValue() {
		var tmp = base.GetObjectCurrentValue();
		tmp["Team"] = m_Entity.GetTeam().ToString();
		tmp["Member"] = m_Entity.GetCurrentMember() + " / " + m_Entity.GetMaxMember();
		tmp["TimeSpawnMember"] = m_Entity.GetTimeSpawnSodier();
		return tmp;
	}

	public override void SpawnSodier()
	{
		base.SpawnSodier();
		var memberType = m_Entity.GetMemberType();
		LCEntity member = null;
		if (m_GameManager.GetObjectPool(memberType, ref member))
		{
			var memberCount = m_Entity.GetCurrentMember();
			memberCount++;
			m_Entity.SetCurrentMember(memberCount);
			member.SetHealth(member.GetMaxHealth());
			member.SetTeam(m_Entity.GetTeam());
			member.SetOwner(m_Entity);
			member.SetTransformPosition(TransformPosition);
			var target = (Vector3)Random.insideUnitCircle * GetColliderRadius();
			target.z = target.y;
			target.y = 0f;
			member.SetTargetPosition(target);
			member.SetEnemyEntity(null);
			member.SetActive(true);
		}
	}

	public override void SpawnAllSodier()
	{
		base.SpawnAllSodier();
		for (int i = 0; i < m_Entity.GetMaxMember(); i++)
		{
			SpawnSodier();
		}
	}

	public override void ReturnObjectPool()
	{
		m_Entity.ReturnObjectPool();
		m_GameManager.SetObjectPool(m_Entity);
	}

	#endregion

	#region FSM

	internal virtual bool IsFullCave()
	{
		return m_Entity.GetCurrentMember() == m_Entity.GetMaxMember();
	}

	#endregion

	#region Getter && Setter

	#endregion

}


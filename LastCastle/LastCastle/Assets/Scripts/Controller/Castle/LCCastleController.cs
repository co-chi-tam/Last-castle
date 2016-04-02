using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LCCastleController : LCBaseController {

	#region Properties

	#endregion

	#region Implementation Monobehaviour

	public override void Init()
	{
		base.Init();

		var idle = new FSMCastleIdleState(this);
		var repairResourceState = new FSMCastleRepairResourceState(this);
		var spawnTroopState = new FSMCastleSpawnTroopState(this);
		var spawnAllTroopState = new FSMCastleSpawnAllTroopState(this);
		var failState = new FSMCastleFailState(this);
		var castleManual = new FSMCastleManualState(this);
		var castleAI = new FSMCastleAIState(this);

		m_FSMManager.RegisterState("CastleIdleState", idle);
		m_FSMManager.RegisterState("CastleRepairResourceState", repairResourceState);
		m_FSMManager.RegisterState("CastleSpawnTroopState", spawnTroopState);
		m_FSMManager.RegisterState("CastleSpawnAllTroopState", spawnAllTroopState);
		m_FSMManager.RegisterState("CastleFailState", failState);
		m_FSMManager.RegisterState("CastleManualState", castleManual);
		m_FSMManager.RegisterState("CastleAIState", castleAI);

		m_FSMManager.RegisterCondition("HaveTroopQueue", HaveTroopQueue);
		m_FSMManager.RegisterCondition("HaveCompleteResource", HaveCompleteResource);
		m_FSMManager.RegisterCondition("IsCastleFail", IsCastleFail);
		m_FSMManager.RegisterCondition("IsManual", IsManual);

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
		tmp["Health Point"] = m_Entity.GetHealth();
		tmp["Wave Index"] = m_Entity.GetWaveIndex();
		tmp["Wave Countdown"] = m_Entity.GetWaveCountDown();
		tmp["Cereal"] = m_Entity.GetCereal();
		tmp["Fish"] = m_Entity.GetFish();
		tmp["Meat"] = m_Entity.GetMeat();
		tmp["Gold"] = m_Entity.GetMetal();
		tmp["Population"] = m_Entity.GetPopulation();
		tmp["Enemy Name"] = m_Entity.GetEnemyEntity() != null ? m_Entity.GetEnemyEntity().GetController().name : "None";
		return tmp;
	}

	public override void SpawnSodier()
	{
		var nextTroop = m_Entity.GetTroopQueue().Dequeue();
		LCEntity troop = null;
		if (m_GameManager.GetObjectPool(nextTroop, ref troop))
		{
			var price = m_GameManager.GetSodierData(nextTroop);
			m_Entity.SetCereal(-price.CostResources[(int)LCEnum.EResourcesType.Cereal]);
			m_Entity.SetFish(-price.CostResources[(int)LCEnum.EResourcesType.Fish]);
			m_Entity.SetMeat(-price.CostResources[(int)LCEnum.EResourcesType.Meat]);
			m_Entity.SetMetal(-price.CostResources[(int)LCEnum.EResourcesType.Metal]);
			m_Entity.SetPopulation(-troop.GetPopulation());
			var postion = (Vector3)Random.insideUnitCircle * GetColliderRadius();
			postion.x += this.TransformPosition.x;
			postion.z += postion.y + this.TransformPosition.z;
			postion.y = this.TransformPosition.y;
			troop.SetHealth(troop.GetMaxHealth());
			troop.SetTransformPosition(postion);
			troop.SetTeam(m_Entity.GetTeam());
			troop.SetTargetPosition(m_Entity.GetEnemyPosition());
			troop.SetEnemyEntity(null);
			troop.SetOwner(m_Entity);
			troop.SetActive(true);
		}
	}

	public override void SpawnAllSodier()
	{
		base.SpawnAllSodier();
		var count = m_Entity.GetTroopQueue().Count;
		for (int i = 0; i < count; i++)
		{
			SpawnSodier();
		}
	}

	public override void AddTroopWave()
	{
		base.AddTroopWave();
		m_Entity.AddTroopWave();
	}

	public override void ReturnObjectPool()
	{
		base.ReturnObjectPool();
		m_GameManager.WasCastleFail(m_Entity);
	}

	#endregion

	#region FSM

	internal override bool HaveTroopQueue()
	{
		return m_Entity.GetTroopCount() > 0 && m_Entity.GetPopulation() > 0;
	}

	internal override bool HaveCompleteResource()
	{ 
		if (m_Entity.GetTroopCount() == 0)
			return false;
		var nextTroop = m_Entity.GetTroopQueue().Peek();
		var result = m_GameManager.CheckSodierPrice(nextTroop, m_Entity.GetResources(), m_Entity.GetPopulation());
		return result;
	}

	internal override bool IsCastleFail()
	{
		return m_Entity.GetActive() == false || m_Entity.GetHealth() <= 0;
	}

	internal virtual bool IsManual()
	{
		return m_Entity.GetCommand() == LCEnum.ECommand.Manual;
	}

	#endregion

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LCCastle : LCEntity {

	#region Properties

	private LCCastleController m_Controller;
	private LCCastleData m_Data;

	protected LCGameManager m_GameManager;
	protected Queue<LCEnum.EEntityType> m_TroopQueue;
	protected LCEntity m_Enemy;
	protected LCEnum.ECommand m_Command;
	protected int m_Cereal;
	protected int m_Fish;
	protected int m_Meat;
	protected int m_Metal;
	protected int m_Population;
	protected int m_HealthPoint;

	protected float m_WaveDelay;
	protected int m_WaveIndex;

	#endregion

	#region Contructor

	public LCCastle(LCBaseController ctrl, LCBaseData data): base (ctrl, data)
	{
		m_Controller = ctrl as LCCastleController;
		m_Data = data as LCCastleData;

		m_TroopQueue = new Queue<LCEnum.EEntityType>();
		m_WaveIndex = 0;
		m_GameManager = LCGameManager.GetInstance();
	}

	#endregion

	#region Main methods

	public override void InitUI()
	{
		base.InitUI();
		if (OnCerealChange != null)
		{
			OnCerealChange(GetCereal(), m_Cereal, 9999);
		}
		if (OnFishChange != null)
		{
			OnFishChange(GetFish(), m_Fish, 9999);
		}
		if (OnMeatChange != null)
		{
			OnMeatChange(GetMeat(), m_Meat, 9999);
		}
		if (OnMetalChange != null)
		{
			OnMetalChange(GetMetal(), m_Metal, 9999);
		}
		if (OnPopulationChange != null)
		{
			OnPopulationChange(GetPopulation(), m_Population, 9999);
		}
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		if (m_Cereal != 0)
		{
			var rice = GetCereal() + m_Cereal;
			m_Data.Resources[(int)LCEnum.EResourcesType.Cereal] = Mathf.Clamp(rice, 0, 9999);
			if (OnCerealChange != null)
			{
				OnCerealChange(GetCereal(), m_Cereal, 9999);
			}
			m_Cereal = 0;
		}

		if (m_Fish != 0)
		{
			var fish = GetFish() + m_Fish;
			m_Data.Resources[(int)LCEnum.EResourcesType.Fish] = Mathf.Clamp(fish, 0, 9999);
			if (OnFishChange != null)
			{
				OnFishChange(GetFish(), m_Fish, 9999);
			}
			m_Fish = 0;
		}

		if (m_Meat != 0)
		{
			var meat = GetMeat() + m_Meat;
			m_Data.Resources[(int)LCEnum.EResourcesType.Meat] = Mathf.Clamp(meat, 0, 9999);
			if (OnMeatChange != null)
			{
				OnMeatChange(GetMeat(), m_Meat, 9999);
			}
			m_Meat = 0;
		}

		if (m_Metal != 0)
		{
			var metal = GetMetal() + m_Metal;
			m_Data.Resources[(int)LCEnum.EResourcesType.Metal] = Mathf.Clamp(metal, 0, 9999);
			if (OnMetalChange != null)
			{
				OnMetalChange(GetMetal(), m_Metal, 9999);
			}
			m_Metal = 0;
		}

		if (m_Population != 0)
		{
			var population = GetPopulation() + m_Population;
			m_Data.Population = Mathf.Clamp(population, 0, 9999);
			if (OnPopulationChange != null)
			{
				OnPopulationChange(GetPopulation(), m_Population, 9999);
			}
			m_Population = 0;
		}
	}

	public override void ApplyDamage(int damage, LCEntity attacker)
	{
		base.ApplyDamage(damage, attacker);

		m_HealthPoint -= damage;
		if (m_HealthPoint != 0)
		{
			var health = GetHealth() + m_HealthPoint;
			SetHealth(health);
			m_HealthPoint = 0;
		}
	}

	public override void AddTroopWave()
	{
		base.AddTroopWave();
		var wave = m_Data.ListWave[m_WaveIndex];
		var random = Random.Range(0, 999) % wave.WaveDatas.Count;
		var troops = wave.WaveDatas[random];
		SetTroopQueue(troops);
		m_WaveDelay = wave.TimeDelay;
		m_WaveIndex = (m_WaveIndex + 1) % m_Data.ListWave.Count;
	}

	#endregion

	#region Getter && Setter

	public override string GetFSMPath() {
		return m_Data.FSMPath;
	}

	public override LCBaseController GetController() {
		return m_Controller;
	}

	public override LCEnum.EEntityType GetEntityType()
	{
		return m_Data.EntityType;
	}

	public override Quaternion GetTransformRotation()
	{
		return m_Controller.TransformRotation;
	}

	public override void SetTransformRotation(Quaternion quat)
	{
		base.SetTransformRotation(quat);
		m_Controller.TransformRotation = quat;
	}

	public override void SetTransformPosition(Vector3 pos)
	{
		base.SetTransformPosition(pos);
		m_Controller.TransformPosition = pos;
	}

	public override Vector3 GetTransformPosition()
	{
		return m_Controller.TransformPosition;
	}

	public override Vector3 GetEnemyPosition()
	{
		return m_Enemy.GetTransformPosition();
	}

	public override void SetEnemyEntity(LCEntity entity)
	{
		m_Enemy = entity;
	}

	public override LCEntity GetEnemyEntity()
	{
		return m_Enemy;
	}

	public override LCEnum.EClass GetTeam()
	{
		return m_Data.Team;
	}

	public override void SetTeam(LCEnum.EClass team)
	{
		base.SetTeam(team);
		m_Data.Team = team;
	}

	public override float GetColliderRadius() {
		return m_Controller.GetColliderRadius();
	}

	public override void SetAnimation(EAnimation anim) {
		m_Controller.SetAnimation(anim);
	}

	public override int GetHealth ()
	{
		return m_Data.CurrentHealthPoint;
	}

	public override void SetHealth (int value)
	{
		base.SetHealth(value);
		m_Data.CurrentHealthPoint = value;
	}

	public override int GetMaxHealth() {
		return m_Data.MaxHealthPoint;
	}

	public override LCEnum.EEntityType[] GetTroopTypes()
	{
		return m_Data.TroopTypes;
	}

	public override int GetCereal()
	{
		return m_Data.Resources[(int)LCEnum.EResourcesType.Cereal];
	}

	public override void SetCereal(int value)
	{
		base.SetCereal(value);
		m_Cereal += value;
	}

	public override int GetFish()
	{
		return m_Data.Resources[(int)LCEnum.EResourcesType.Fish];
	}

	public override void SetFish(int value)
	{
		base.SetFish(value);
		m_Fish += value;
	}

	public override int GetMeat()
	{
		return m_Data.Resources[(int)LCEnum.EResourcesType.Meat];
	}

	public override void SetMeat(int value)
	{
		base.SetMeat(value);
		m_Meat += value;
	}

	public override int GetMetal()
	{
		return m_Data.Resources[(int)LCEnum.EResourcesType.Metal];
	}

	public override void SetMetal(int value)
	{
		base.SetMetal(value);
		m_Metal += value;
	}

	public override int GetPopulation()
	{
		return m_Data.Population;
	}

	public override void SetPopulation(int value)
	{
		base.SetPopulation(value);
		m_Population = value;
	}

	public override int GetResourceBonus()
	{
		return m_Data.ResourceBonus;
	}

	public override int GetHealthPointBonus()
	{
		return m_Data.HealthPointBonus;
	}

	public override float GetMoveSpeedBonus()
	{
		return m_Data.MoveSpeedBonus;
	}

	public override int GetTroopCount()
	{
		return m_TroopQueue.Count;
	}

	public override Queue<LCEnum.EEntityType> GetTroopQueue()
	{
		base.GetTroopQueue();
		return m_TroopQueue;
	}

	public override void SetTroopQueue(params int[] troops)
	{
		base.SetTroopQueue(troops);
		for (int i = 0; i < troops.Length; i++)
		{
			var index = troops[i] < m_Data.TroopTypes.Length ? troops[i] : 0;
			var type = m_Data.TroopTypes[index];
			m_TroopQueue.Enqueue(type);
		}
	}

	public override void SetSingleTroop(int troop)
	{
		if (troop >= m_Data.TroopTypes.Length)
		{
			return;
		}
		base.SetSingleTroop(troop);
		var type = m_Data.TroopTypes[troop];
		var result = m_GameManager.CheckSodierPrice(type, GetResources(), GetPopulation());
		if (result)
		{
			m_TroopQueue.Enqueue(type);
		}
	}

	public override void SetCommand(LCEnum.ECommand value)
	{
		base.SetCommand(value);
		m_Command = value;
	}

	public override LCEnum.ECommand GetCommand()
	{
		return m_Command;
	}

	public override float GetWaveCountDown()
	{
		return m_WaveDelay;
	}

	public override int GetWaveIndex()
	{
		return m_WaveIndex;
	}

	public override int GetGoldWinBonus()
	{
		return m_Data.GoldWinBonus;
	}

	public override int[] GetResources()
	{
		return m_Data.Resources;
	}

	#endregion

}

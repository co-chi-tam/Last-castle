using UnityEngine;
using System.Collections;

public class LCSodier : LCEntity {

	#region Properties

	private LCSodierController m_Controller;
	private LCSodierData m_Data;

	protected LCEntity m_AttackerEntity;
	protected LCEntity m_EnemyEntity;
	protected Vector3 m_TargetPosition;
	protected int m_HealthPoint;

	#endregion

	#region Contructor

	public LCSodier(LCBaseController ctrl, LCBaseData data): base (ctrl, data)
	{
		m_Controller = ctrl as LCSodierController;
		m_Data = data as LCSodierData;
	}

	#endregion

	#region Main methods

	public override void ApplyDamage(int damage, LCEntity attacker)
	{
		base.ApplyDamage(damage, attacker);
		m_HealthPoint -= damage;

		if (m_HealthPoint != 0 && GetHealth() > 0f)
		{
			var health = GetHealth() + m_HealthPoint;
			SetHealth(health);
			m_HealthPoint = 0;
			if (GetHealth() <= 0f)
			{
				SetAttacker(attacker);
			}
		}

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
		return m_EnemyEntity.GetTransformPosition();
	}

	public override void SetEnemyEntity(LCEntity entity)
	{
		m_EnemyEntity = entity;
	}

	public override LCEntity GetEnemyEntity()
	{
		return m_EnemyEntity;
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

	public override float GetMoveSpeed()
	{
		return m_Data.MoveSpeed;
	}

	public override float GetRotationSpeed()
	{
		return 5f;
	}

	public override float GetColliderRadius() {
		return m_Controller.GetColliderRadius();
	}

	public override void SetAnimation(EAnimation anim) {
		m_Controller.SetAnimation(anim);
	}

	public override float GetDetectRange() {
		return m_Data.DetectRange;
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

	public override float GetAttackRange()
	{
		return m_Data.AttackRange;
	}

	public override int GetAttackDamage()
	{
		return m_Data.AttackDamage;
	}

	public override float GetAttackSpeed()
	{
		return m_Data.AttackSpeed;
	}

	public override Vector3 GetTargetPosition()
	{
		return m_TargetPosition;
	}

	public override void SetTargetPosition(Vector3 position)
	{
		m_TargetPosition = position;
	}

	public override LCEntity GetAttacker()
	{
		return m_AttackerEntity;
	}

	public override void SetAttacker(LCEntity value)
	{
		base.SetAttacker(value);
		m_AttackerEntity = value;
	}

	public override void SetCereal(int value)
	{
		base.SetCereal(value);
		m_Owner.SetCereal(value);
	}

	public override void SetFish(int value)
	{
		base.SetFish(value);
		m_Owner.SetFish(value);
	}

	public override void SetMeat(int value)
	{
		base.SetMeat(value);
		m_Owner.SetMeat(value);
	}

	public override void SetMetal(int value)
	{
		base.SetMetal(value);
		m_Owner.SetMetal(value);
	}

	public override int GetPopulation()
	{
		return m_Data.Population;
	}

	public override int[] GetCostResources()
	{
		return m_Data.CostResources;
	}

	public override int[] GetDropResources()
	{
		return m_Data.DropResources;
	}

	#endregion

}

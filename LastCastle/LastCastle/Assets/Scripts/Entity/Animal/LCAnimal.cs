using UnityEngine;
using System.Collections;

public class LCAnimal : LCSodier
{
	#region Properties

	private LCAnimalController m_Controller;
	private LCAnimalData m_Data;

	#endregion

	#region Contructor

	public LCAnimal(LCBaseController ctrl, LCBaseData data): base (ctrl, data)
	{
		m_Controller = ctrl as LCAnimalController;
		m_Data = data as LCAnimalData;
	}

	#endregion

	#region Main methods

	public override void ReturnObjectPool()
	{
		base.ReturnObjectPool();
		if (GetOwner() != null)
		{
			var memberCount = GetOwner().GetCurrentMember();
			memberCount -= 1;
			GetOwner().SetCurrentMember(memberCount);
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

	public override int[] GetDropResources()
	{
		return m_Data.DropResources;
	}

	#endregion

}


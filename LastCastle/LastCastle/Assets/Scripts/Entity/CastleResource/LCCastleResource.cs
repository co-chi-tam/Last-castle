using UnityEngine;
using System.Collections;

public class LCCastleResource : LCEntity
{
	#region Properties

	private LCCastleResourceController m_Controller;
	private LCCastleResourceData m_Data;

	#endregion

	#region Contructor

	public LCCastleResource(LCBaseController ctrl, LCBaseData data): base (ctrl, data)
	{
		m_Controller = ctrl as LCCastleResourceController;
		m_Data = data as LCCastleResourceData;
	}

	#endregion

	#region Main methods

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

	public override int[] GetResourceCollections()
	{
		return m_Data.ResourceCollects;
	}

	public override float GetTimeCollect()
	{
		return m_Data.TimeCollect;
	}

	#endregion
}


using UnityEngine;
using System.Collections;

public class LCCave : LCEntity {

	#region Properties

	private LCCaveController m_Controller;
	private LCCaveData m_Data;

	#endregion

	#region Contructor

	public LCCave(LCBaseController ctrl, LCBaseData data): base (ctrl, data)
	{
		m_Controller = ctrl as LCCaveController;
		m_Data = data as LCCaveData;
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
		return LCEnum.EClass.Natural;
	}

	public override LCEnum.EEntityType GetMemberType()
	{
		return m_Data.MemberType;
	}

	public override void SetCurrentMember(int value)
	{
		m_Data.CurrentMember = value;
	}

	public override int GetCurrentMember()
	{
		return m_Data.CurrentMember;
	}

	public override int GetMaxMember()
	{
		return m_Data.MaxMember;
	}

	public override float GetTimeSpawnSodier()
	{
		return m_Data.TimeSpawnSodier;
	}

	#endregion

}


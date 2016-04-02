using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LCCastleResourceController : LCBaseController
{
	#region Properties

	#endregion

	#region Implementation Monobehaviour

	public override void Init()
	{
		base.Init();

		var idle = new FSMResourceIdleState(this);
		var resourceWaitingState = new FSMResourceWaitingStateState(this);
		var resourceCollectState = new FSMResourceCollectState(this);

		m_FSMManager.RegisterState("ResourceIdleState", idle);
		m_FSMManager.RegisterState("ResourceWaitingState", resourceWaitingState);
		m_FSMManager.RegisterState("ResourceCollectState", resourceCollectState);

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
		var resources = m_Entity.GetDropResources();
		for (int i = 0; i < resources.Length; i++)
		{
			var type = (LCEnum.EResourcesType)i;
			tmp[type.ToString()] = resources[i].ToString();
		}
		tmp["Time Collect"] = m_Entity.GetTimeCollect();
		tmp["Owner"] = m_Entity.GetOwner() != null ? m_Entity.GetOwner().GetController().name : "None";
		return tmp;
	}

	public override void CompleteCollectResource()
	{
		base.CompleteCollectResource();
		var resources = m_Entity.GetResourceCollections();
		for (int i = 0; i < resources.Length; i++)
		{
			var type = (LCEnum.EResourcesType)i;
			var amount = resources[i];
			switch (type)
			{
				case LCEnum.EResourcesType.Cereal:
					m_Entity.GetOwner().SetCereal(amount);
					break;
				case LCEnum.EResourcesType.Fish:
					m_Entity.GetOwner().SetFish(amount);
					break;
				case LCEnum.EResourcesType.Meat:
					m_Entity.GetOwner().SetMeat(amount);
					break;
				case LCEnum.EResourcesType.Metal:
					m_Entity.GetOwner().SetMetal(amount);
					break;
			}
		}
	}

	#endregion

	#region FSM

	#endregion

	#region Getter && Setter

	#endregion

}


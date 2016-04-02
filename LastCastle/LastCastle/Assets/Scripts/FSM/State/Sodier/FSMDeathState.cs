using UnityEngine;
using System.Collections;
using FSM;

public class FSMDeathState : FSMBaseState
{
	public FSMDeathState(LCBaseController controller) : base (controller)
	{

	}

	public override void StartState()
	{
		m_Controller.SetAnimation(EAnimation.Death);
		m_Controller.SetActive(false);
		m_Controller.CallBackEvent("OnDealth");
		m_Controller.ReturnObjectPool();
		m_Controller.DropResources();
	}

	public override void UpdateState()
	{

	}

	public override void ExitState()
	{
		
	}
}

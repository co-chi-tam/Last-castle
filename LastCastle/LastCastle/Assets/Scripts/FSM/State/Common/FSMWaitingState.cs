using UnityEngine;
using System.Collections;

public class FSMWaitingState : FSMBaseState
{
	public FSMWaitingState(LCBaseController controller) : base (controller)
	{

	}

	public override void StartState()
	{
		m_Controller.SetAnimation(EAnimation.Idle);
	}

	public override void UpdateState()
	{

	}

	public override void ExitState()
	{

	}
}
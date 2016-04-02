using UnityEngine;
using FSM;

public class FSMMoveState : FSMBaseState {

	public FSMMoveState(LCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.SetAnimation(EAnimation.Run);
		m_Controller.CallBackEvent("OnMove");
	}

	public override void UpdateState() {
		m_Controller.MoveToTarget();
	}

	public override void ExitState()
	{

	}
}

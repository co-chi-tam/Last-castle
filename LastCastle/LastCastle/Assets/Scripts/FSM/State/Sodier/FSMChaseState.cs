using UnityEngine;
using FSM;

public class FSMChaseState : FSMBaseState {

	public FSMChaseState(LCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.SetAnimation(EAnimation.Run);
		m_Controller.CallBackEvent("OnChase");
	}

	public override void UpdateState() {
		m_Controller.MoveToEnemy();
	}

	public override void ExitState()
	{

	}
}

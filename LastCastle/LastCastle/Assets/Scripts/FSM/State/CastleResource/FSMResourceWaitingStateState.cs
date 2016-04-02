using UnityEngine;
using FSM;

public class FSMResourceWaitingStateState : FSMBaseState {

	public FSMResourceWaitingStateState(LCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.SetWaitingTime(m_Controller.GetTimeCollect());
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}

}

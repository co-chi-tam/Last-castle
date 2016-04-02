using UnityEngine;
using FSM;

public class FSMResourceCollectState : FSMBaseState {

	public FSMResourceCollectState(LCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.CompleteCollectResource();
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}

}

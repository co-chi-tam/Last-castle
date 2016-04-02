using UnityEngine;
using FSM;

public class FSMCastleWaitingState : FSMBaseState {

	public FSMCastleWaitingState(LCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.SetWaitingTime(m_Controller.GetTimeSpawnMember());
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}

}

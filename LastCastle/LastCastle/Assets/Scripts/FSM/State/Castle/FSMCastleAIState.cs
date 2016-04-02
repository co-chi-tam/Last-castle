using UnityEngine;
using FSM;

public class FSMCastleAIState : FSMBaseState {

	public FSMCastleAIState(LCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.AddTroopWave();
		m_Controller.SetWaitingTime(m_Controller.GetWaveCountDown());
	}

	public override void UpdateState() {
		
	}

	public override void ExitState()
	{
		
	}

}

using UnityEngine;
using FSM;

public class FSMCastleSpawnTroopState : FSMBaseState {

	public FSMCastleSpawnTroopState(LCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.SpawnSodier();
	}

	public override void UpdateState() {
		
	}

	public override void ExitState()
	{
		
	}

}

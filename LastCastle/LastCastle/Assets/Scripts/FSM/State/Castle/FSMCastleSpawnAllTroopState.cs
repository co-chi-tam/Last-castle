using UnityEngine;
using FSM;

public class FSMCastleSpawnAllTroopState : FSMBaseState
{

	public FSMCastleSpawnAllTroopState(LCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.SpawnAllSodier();
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}
}


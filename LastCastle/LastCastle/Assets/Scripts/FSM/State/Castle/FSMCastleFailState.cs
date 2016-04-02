using UnityEngine;
using FSM;

public class FSMCastleFailState : FSMBaseState {

	public FSMCastleFailState(LCBaseController controller) : base (controller)
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

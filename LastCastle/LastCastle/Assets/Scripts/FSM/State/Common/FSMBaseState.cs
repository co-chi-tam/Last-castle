using UnityEngine;
using System.Collections;
using FSM;

public class FSMBaseState : IState {

	protected LCBaseController m_Controller;

	public FSMBaseState(IContext context)
	{
		m_Controller = context as LCBaseController;
	}
	
	public virtual void StartState()
	{

	}
	
	public virtual void UpdateState()
	{
		
	}
	
	public virtual void ExitState()
	{
		
	}

}

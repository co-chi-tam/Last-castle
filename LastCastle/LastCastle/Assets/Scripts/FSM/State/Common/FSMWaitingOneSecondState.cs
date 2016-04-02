using UnityEngine;
using System.Collections;

public class FSMWaitingOneSecondState : FSMBaseState
{
	public FSMWaitingOneSecondState(LCBaseController controller) : base (controller)
	{
		
	}
	
	public override void StartState()
	{
		var random = Mathf.PerlinNoise (Time.time, Time.time) * 2f; // Random.Range(0, 9999) % 2;
		m_Controller.SetAnimation(EAnimation.Idle);
		m_Controller.SetWaitingTime (1f);
	}
	
	public override void UpdateState()
	{
		
	}
	
	public override void ExitState()
	{
		
	}
}

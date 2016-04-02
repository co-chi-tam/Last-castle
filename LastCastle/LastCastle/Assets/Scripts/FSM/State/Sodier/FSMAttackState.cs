using UnityEngine;
using FSM;

public class FSMAttackState : FSMBaseState {

	private float m_AttackSpeed = 0f;

	public FSMAttackState(LCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.SetAnimation(EAnimation.Attack);
		m_Controller.CallBackEvent("OnAttack");
		m_AttackSpeed = m_Controller.GetEntity().GetAttackSpeed();
	}

	public override void UpdateState() {
		if (m_Controller.GetEnemyEntity() != null)
		{
			var enemyPos = m_Controller.GetEnemyPosition();
			m_Controller.LookAtRotation(enemyPos);

			m_AttackSpeed -= Time.deltaTime;
			if (m_AttackSpeed <= 0f)
			{
				var damage = m_Controller.GetEntity().GetAttackDamage();
				m_Controller.GetEnemyEntity().ApplyDamage(damage, m_Controller.GetEntity());
				m_AttackSpeed = m_Controller.GetEntity().GetAttackSpeed();
			}
		}
	}

	public override void ExitState()
	{
		m_AttackSpeed = m_Controller.GetEntity().GetAttackSpeed();
	}

}

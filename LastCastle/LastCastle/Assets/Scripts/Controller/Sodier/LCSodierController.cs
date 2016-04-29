using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LCSodierController : LCBaseController
{

	#region Properties

	protected int m_ColliderLayerMask;

	#endregion

	#region Implementation Monobehaviour

	public override void Init ()
	{
		base.Init ();

		m_ColliderLayerMask = 1 << (int)LCEnum.ELayer.Castle | 1 << (int)LCEnum.ELayer.Troop | 1 << (int)LCEnum.ELayer.Animal;

		var idle = new FSMIdleState (this);
		var moveState = new FSMMoveState (this);
		var chaseState = new FSMChaseState (this);
		var attackState = new FSMAttackState (this);
		var deathState = new FSMDeathState (this);

		m_FSMManager.RegisterState ("IdleState", idle);
		m_FSMManager.RegisterState ("MoveState", moveState);
		m_FSMManager.RegisterState ("ChaseState", chaseState);
		m_FSMManager.RegisterState ("AttackState", attackState);
		m_FSMManager.RegisterState ("DeathState", deathState);


		m_FSMManager.RegisterCondition ("DidMoveToPosition", DidMoveToPosition);
		m_FSMManager.RegisterCondition ("HaveEnemy", HaveEnemy);
		m_FSMManager.RegisterCondition ("HaveFoundEnemy", HaveFoundEnemy);
		m_FSMManager.RegisterCondition ("IsEnemyInRange", IsEnemyInRange);
		m_FSMManager.RegisterCondition ("IsEnemyDeath", IsEnemyDeath);
		m_FSMManager.RegisterCondition ("IsDeath", IsDeath);

		m_FSMManager.LoadFSM (m_Entity.GetFSMPath ());
	}

	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
		m_Entity.Update (Time.fixedDeltaTime);
		m_FSMManager.UpdateState ();
	}

	protected override void OnDrawGizmos ()
	{
		base.OnDrawGizmos ();
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (TransformPosition, m_Entity.GetDetectRange ());
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (TransformPosition, GetColliderRadius () + m_Entity.GetAttackRange ());
		Gizmos.color = Color.white;
		Gizmos.DrawLine (TransformPosition, m_Entity.GetTargetPosition ());
	}

	#endregion

	#region Main methods

	public override Dictionary<string, object> GetObjectCurrentValue ()
	{
		var tmp = base.GetObjectCurrentValue ();
		tmp ["Team"] = m_Entity.GetTeam ().ToString ();
		tmp ["Owner"] = m_Entity.GetOwner () != null ? m_Entity.GetOwner ().GetController ().name : "None";
		tmp ["Health Point"] = m_Entity.GetHealth ();
		tmp ["Attack Range"] = m_Entity.GetAttackRange ();
		tmp ["Attack Speed"] = m_Entity.GetAttackSpeed ();
		tmp ["Attack Damage"] = m_Entity.GetAttackDamage ();
		tmp ["Move Speed"] = m_Entity.GetMoveSpeed ();
		tmp ["Attack Damage"] = m_Entity.GetAttackDamage ();
		tmp ["Enemy Name"] = m_Entity.GetEnemyEntity () != null ? m_Entity.GetEnemyEntity ().GetController ().name : "None";
		return tmp;
	}

	public override void LookAtRotation (Vector3 rotation)
	{
		base.LookAtRotation (rotation);
		rotation.y = 0f;
		var mPos = m_Transform.position;
		mPos.y = 0f;
		var direction = mPos - rotation;
		if (direction == Vector3.zero)
			return;
		Quaternion rot = Quaternion.LookRotation (direction);
		m_Transform.rotation = Quaternion.Slerp (m_Transform.rotation, rot, Time.fixedDeltaTime * m_Entity.GetRotationSpeed ());
	}

	public override void MoveToPosition (Vector3 position)
	{
		base.MoveToPosition (position);
		position.y = 0f;
		var direction = position - m_Transform.position;
		if (direction == Vector3.zero)
			return;
		var nextPosition = m_Transform.position + direction.normalized * m_Entity.GetMoveSpeed () * Time.fixedDeltaTime;
		m_Transform.position = nextPosition;
		LookAtRotation (position);
	}

	public override void DropResources ()
	{
		base.DropResources ();
		var resources = m_Entity.GetDropResources ();
		var attacker = m_Entity.GetAttacker ();
		for (int i = 0; i < resources.Length; i++) {
			var type = (LCEnum.EResourcesType)i;
			var amount = resources [i];
			switch (type) {
			case LCEnum.EResourcesType.Cereal:
				attacker.SetCereal (amount);
				break;
			case LCEnum.EResourcesType.Fish:
				attacker.SetFish (amount);
				break;
			case LCEnum.EResourcesType.Meat:
				attacker.SetMeat (amount);
				break;
			case LCEnum.EResourcesType.Metal:
				attacker.SetMetal (amount);
				break;
			}
		}
	}

	#endregion

	#region FSM

	internal override bool HaveEnemy ()
	{
		return m_Entity.GetEnemyEntity () != null;
	}

	internal override bool HaveFoundEnemy ()
	{
		var enemy = m_Entity.GetEnemyEntity ();
		if (enemy != null) { 
			if (enemy.GetHealth () <= 0 || enemy.GetActive () == false) {
				return false;
			}
			return true;
		}
		var mPos = TransformPosition;
		var colliders = Physics.OverlapSphere (mPos, m_Entity.GetDetectRange (), m_ColliderLayerMask);
		if (colliders.Length == 0)
			return false;
		for (int i = 0; i < colliders.Length; i++) {
			var target = m_GameManager.GetEntityByName (colliders [i].name);
			if (target == null || target.GetActive () == false || target == this.GetEntity ()) {
				continue;
			} else {
				if (target.GetTeam () != m_Entity.GetTeam ()) {
					m_Entity.SetEnemyEntity (target);
					return true;
				}
			}
		}
		return false;
	}

	internal override bool DidMoveToPosition ()
	{
		var distance = (TransformPosition - m_Entity.GetTargetPosition ()).sqrMagnitude;
		var range = 0.05f;
		return distance <= range * range; 
	}

	internal override bool IsEnemyInRange ()
	{
		var enemy = m_Entity.GetEnemyEntity ();
		if (enemy != null) {
			var distance = (TransformPosition - m_Entity.GetEnemyPosition ()).sqrMagnitude;
			var range = enemy.GetColliderRadius () + m_Entity.GetAttackRange ();
			return distance <= range * range; 
		}
		return true;
	}

	internal override bool IsEnemyDeath ()
	{
		var enemy = m_Entity.GetEnemyEntity ();
		if (enemy == null) {
			return true;
		}
		var result = enemy.GetHealth () <= 0 || enemy.GetActive () == false;
		if (result) {
			m_Entity.SetEnemyEntity (null);
		}
		return result;
	}

	internal override bool IsDeath ()
	{
		return m_Entity.GetHealth () <= 0 || m_Entity.GetActive () == false;
	}

	#endregion

	#region Getter && Setter

	#endregion

}

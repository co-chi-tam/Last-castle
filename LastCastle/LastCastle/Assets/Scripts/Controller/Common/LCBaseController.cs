using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

public enum EAnimation:int {
	Idle    = 0,
	Run     = 1,
	Attack	= 2,
	Death     = 10
}

public class LCBaseController : CMonoBehaviour, IContext
{
	#region Property

	protected LCEntity m_Entity;
	protected Transform m_Transform;
	protected Animator m_AnimatorController;
	protected CapsuleCollider m_Collider;
	protected LCGameManager m_GameManager;
	protected FSMManager m_FSMManager;
	protected float m_WaitingTime = 3f;
	protected int m_LayerDetect;

	public virtual Vector3 TransformPosition {
		get { return m_Transform.position; }
		set { m_Transform.position = value; }
	}

	public virtual Quaternion TransformRotation {
		get { return m_Transform.rotation; }
		set { m_Transform.rotation = value; }
	}

	protected Dictionary<string, object> m_ObjectCurrentvalue;

	#endregion

	#region Implement Monobehaviour

	public virtual void Init() {
		m_Transform	= this.transform;
		SetStartPosition (m_Transform.position);

		m_AnimatorController = this.GetComponent<Animator> ();
		m_Collider 			 = this.GetComponent<CapsuleCollider> ();
		m_ObjectCurrentvalue = new Dictionary<string, object>();

		m_LayerDetect = 1 << (int)LCEnum.ELayer.Plane;

		m_GameManager 	= LCGameManager.GetInstance();
		m_FSMManager 	= new FSMManager();

		var waiting 		= new FSMWaitingState (this);
		var waitingOne 		= new FSMWaitingOneSecondState (this);
		var waitingOne2Three = new FSMWaitingOne2ThreeSecondState (this);
		var waitingThree2Five = new FSMWaitingThree2FiveSecondState (this);

		m_FSMManager.RegisterState("WaitingState", waiting);
		m_FSMManager.RegisterState("WaitingOneSecondState", waitingOne);
		m_FSMManager.RegisterState("WaitingOne2ThreeSecondState", waitingOne2Three);
		m_FSMManager.RegisterState("WaitingThree2FiveSecondState", waitingThree2Five);

		m_FSMManager.RegisterCondition("IsActive", IsActive);
		m_FSMManager.RegisterCondition("CountdownWaitingTime", CountdownWaitingTime);
		m_FSMManager.RegisterCondition("WasGameEnd", WasGameEnd);
	}

	protected virtual void OnEnable() {
		if (m_AnimatorController != null)
		{
			m_AnimatorController.enabled = true;
		}
		if (m_Collider != null)
		{
			m_Collider.enabled = true;
		}
	}

	protected virtual void OnDisable() {
		if (m_AnimatorController != null)
		{
			m_AnimatorController.enabled = false;
		}
		if (m_Collider != null)
		{
			m_Collider.enabled = false;
		}
	}

	protected virtual void Start()
	{
		
    }

	protected virtual void FixedUpdate() {
		
	}

	protected virtual void Update() {
		
	}

	protected virtual void LateUpdate() {
		
	}

	public virtual void OnBecameVisible() {
		m_Entity.VisibleObject(true);
	}

	public virtual void OnBecameInvisible() {
		m_Entity.VisibleObject(false);
	}

	protected virtual void OnDestroy() {
		
	}

	protected virtual void OnApplicationQuit() {
		
	}

	protected virtual void OnApplicationFocus(bool focus) {
		
	}

	protected virtual void OnDrawGizmos() {
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere (TransformPosition, GetColliderRadius());
	}

	#endregion

	#region Main Method

	public virtual Dictionary<string, object> GetObjectCurrentValue() {
		m_ObjectCurrentvalue["Active"] = GetActive();
		m_ObjectCurrentvalue["State Name"] = m_FSMManager != null ? m_FSMManager.CurrentStateName : "None";
		m_ObjectCurrentvalue["Command"] = m_Entity.GetCommand().ToString();
		return m_ObjectCurrentvalue;
	}

	public virtual void InitUI() {

	}

	public virtual void UpdateWaitingCommand(float dt) {

	}

	public virtual void ActiveAction(int index) {

	}

	public virtual void MoveToTarget()
	{
		MoveToPosition(m_Entity.GetTargetPosition());
	}

	public virtual void MoveToEnemy()
	{
		if (m_Entity.GetEnemyEntity() == null)
			return;
		MoveToPosition(m_Entity.GetEnemyPosition());
	}

	public virtual void MoveToPosition(Vector3 position)
	{
		CallBackEvent("OnMove");
		if (m_Entity.GetVisibleObject() == false)
		{
			VisibleObject(true);
		}
	}

	public virtual void LookAtRotation(Vector3 rotation) {

	}

	public virtual void ApplyDamage(int damage, LCEntity attacker) {
		m_Entity.ApplyDamage(damage, attacker);
	}

	public virtual void ReturnObjectPool() {
		m_Entity.ReturnObjectPool();
		m_GameManager.SetObjectPool(m_Entity);
	}

	public virtual void VisibleObject(bool value) {
		m_Entity.VisibleObject(value);
		var childCount = m_Transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			m_Transform.GetChild(i).gameObject.SetActive(value);
		}
	}

	public virtual void DropResources() {

	}

	public virtual void CallBackEvent(string name) {
		m_Entity.CallBackEvent(name);
	}

	public virtual void SpawnSodier()
	{
		
	}

	public virtual void SpawnAllSodier()
	{

	}

	public virtual void CompleteCollectResource()
	{
		
	}

	#endregion
		
	#region FSM

	internal virtual bool IsActive()
	{
		return GetActive();
	}

	internal virtual bool IsDeath() {
		return GetActive() == false;
	}

	internal virtual bool CountdownWaitingTime() {
		m_WaitingTime -= Time.deltaTime;
		return m_WaitingTime <= 0f;     
	}

	internal virtual bool IsEnemyDeath()
	{
		return false;  
	}

	internal virtual bool HaveEnemy()
	{
		return m_Entity.GetEnemyEntity() != null && m_Entity.GetActive() == true;
	}

	internal virtual bool HaveFoundEnemy()
	{
		return m_Entity.GetEnemyEntity() != null && m_Entity.GetActive() == true;
	}

	internal virtual bool IsEnemyInRange()
	{
		return m_Entity.GetEnemyEntity() != null;
	}

	internal virtual bool DidMoveToPosition()
	{
		return false;
	}

	internal virtual bool HaveTroopQueue()
	{
		return false;
	}

	internal virtual bool HaveCompleteResource()
	{
		return false;
	}

	internal virtual bool IsCastleFail()
	{
		return false;
	}

	internal virtual bool WasGameEnd()
	{
		return m_GameManager.OnPlaying == false;
	}

	#endregion

    #region Getter & Setter

	public virtual string GetStateName() {
		return m_FSMManager.CurrentStateName;
	}

	public virtual void SetEntity(LCEntity entity) {
		m_Entity = entity;
	}

	public virtual LCEntity GetEntity() {
		return m_Entity;
	}

	public virtual float GetColliderRadius() {
		if (m_Collider == null)
			return 0f;
		return m_Collider.radius;
	}

	public virtual void SetActive(bool value) {
		m_Entity.SetActive(value);
		this.gameObject.SetActive(value);
    }

    public virtual bool GetActive() {
		return m_Entity.GetActive();
    }

	public virtual void SetStartPosition(Vector3 pos) {
		m_Entity.SetStartPosition(pos);
		this.transform.position = pos;
	}

	public virtual Vector3 GetStartPosition() {
		return m_Entity.GetStartPosition();
	}

	public virtual LCEnum.EEntityType GetEntityType() {
		return m_Entity.GetEntityType();
	}
	
	public virtual void SetWaitingTime(float time) {
		m_WaitingTime = time;
	}
	
	public virtual void SetAnimation(EAnimation anim) {
		
	}

	public virtual LCEntity GetEnemyEntity()
	{
		return m_Entity.GetEnemyEntity();
	}

	public virtual Vector3 GetEnemyPosition()
	{
		return m_Entity.GetEnemyPosition();
	}

	public virtual float GetTimeCollect()
	{
		return m_Entity.GetTimeCollect();
	}

	public virtual float GetTimeSpawnMember()
	{
		return m_Entity.GetTimeSpawnSodier();
	}

	public virtual void AddTroopWave()
	{
		
	}

	public virtual float GetWaveCountDown()
	{
		return m_Entity.GetWaveCountDown();
	}

	#endregion

}
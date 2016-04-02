using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LCEntity : PropertyReflection {

	#region Properties

	protected bool m_IsActive;
	protected bool m_IsVisible;

//	protected ObjectProperty<float> m_OffsetSpeed;
//	protected ObjectProperty<int> m_HealthPoint;

	protected Dictionary<string, Action> m_TriggerEvents;
	protected LCEntity m_Owner;

	#endregion

	#region Event 

	protected event Action OnIdleEvent;
	protected event Action OnFindEnemyEvent;
	protected event Action OnMoveEvent;
	protected event Action OnChaseEvent;
	protected event Action OnApplyDamageEvent;
	protected event Action OnAttackEvent;
	protected event Action OnAliveEvent;
	protected event Action OnDeathEvent;
	protected event Action OnDayEvent;
	protected event Action OnMidDayEvent;
	protected event Action OnNightEvent;
	protected event Action OnMidNightEvent;

	public Action<int, int, int> OnHealthChange;
	public Action<int, int, int> OnCerealChange;
	public Action<int, int, int> OnFishChange;
	public Action<int, int, int> OnMeatChange;
	public Action<int, int, int> OnMetalChange;
	public Action<int, int, int> OnPopulationChange;
	public Action<int, int, int> OnGoldChange;

	public virtual void CallBackEvent(string name) {
		if (m_TriggerEvents.ContainsKey(name))
		{
			var evnt = m_TriggerEvents[name];
			if (evnt != null)
			{
				evnt.Invoke();
			}
		}
	}

	public virtual bool AddEventListener(string name, Action evnt) {
		if (m_TriggerEvents.ContainsKey(name))
		{
			m_TriggerEvents[name] += evnt;
			return true;
		}
		return false;
	}

	public virtual bool RemoveEventListener(string name, Action evnt) {
		if (m_TriggerEvents.ContainsKey(name))
		{
			m_TriggerEvents[name] -= evnt;
			return true;
		}
		return false;
	}

	protected virtual void LoadEventCallBack() {
		m_TriggerEvents.Add("OnIdle", OnIdleEvent);
		m_TriggerEvents.Add("OnFindEnemy", OnFindEnemyEvent);
		m_TriggerEvents.Add("OnMove", OnMoveEvent);
		m_TriggerEvents.Add("OnChase", OnChaseEvent);
		m_TriggerEvents.Add("OnApplyDamage", OnApplyDamageEvent);
		m_TriggerEvents.Add("OnAttack", OnAttackEvent);
		m_TriggerEvents.Add("OnAlive", OnAliveEvent);
		m_TriggerEvents.Add("OnDeath", OnDeathEvent);
		m_TriggerEvents.Add("OnDay", OnDayEvent);
		m_TriggerEvents.Add("OnMidDay", OnMidDayEvent);
		m_TriggerEvents.Add("OnNight", OnNightEvent);
		m_TriggerEvents.Add("OnMidNight", OnMidNightEvent);
	}

	#endregion

	#region Contructor

	public LCEntity(LCBaseController ctrl, LCBaseData data)
	{
		m_TriggerEvents = new Dictionary<string, Action>();
		LoadEventCallBack();
//
//		m_OffsetSpeed = new ObjectProperty<float>("OffsetSpeed", 1f);
//		m_HealthPoint = new ObjectProperty<int>("HealthPoint");

//		RegisterProperty(m_OffsetSpeed);
//		RegisterProperty(m_HealthPoint);
	}

	#endregion

	#region Main methods

	public virtual void InitUI() {

	}

	public virtual void Update(float dt) {
		
	}

	public virtual void ApplyDamage(int damage, LCEntity attacker) {

	}

	public virtual void ReturnObjectPool(){

	}

	public virtual void VisibleObject(bool value){
		m_IsVisible = value;
	}

	public virtual string SerializeJSON()
	{
		return string.Empty;
	}

	#endregion

	#region Common

	public virtual void SetTriggerEvent(string name, Action evnt) {
		m_TriggerEvents.Add(name, evnt);
	}

	public virtual Action GetTriggerEvent(string name) {
		return m_TriggerEvents[name];
	}

	public virtual string GetFSMPath() {
		return string.Empty;
	}

	public virtual LCBaseController GetController() {
		return null;
	}

	public virtual void SetActive(bool value) {
		m_IsActive = value;
		if (GetController() != null)
		{
			GetController().gameObject.SetActive(value);
		}
	}

	public virtual bool GetActive() {
		return m_IsActive;
	}

	public virtual LCEnum.EEntityType GetEntityType()
	{
		return LCEnum.EEntityType.None;
	}

	public virtual Vector3 GetStartPosition()
	{
		return Vector3.zero;
	}

	public virtual void SetStartPosition(Vector3 pos) {
		
	}

	public virtual void SetTransformPosition(Vector3 pos) {

	}

	public virtual Vector3 GetTransformPosition() {
		return Vector3.zero;
	}

	public virtual void SetTransformRotation(Quaternion quat) {

	}

	public virtual Quaternion GetTransformRotation() {
		return Quaternion.identity;
	}

	public virtual bool GetVisibleObject() {
		return m_IsVisible;
	}

	public virtual LCEnum.EClass GetTeam()
	{
		return LCEnum.EClass.None;
	}

	public virtual void SetTeam(LCEnum.EClass team)
	{

	}

	#endregion

	#region Sodier

	public virtual Vector3 GetEnemyPosition()
	{
		return Vector3.zero;
	}

	public virtual void SetEnemyEntity(LCEntity entity)
	{
		
	}

	public virtual LCEntity GetEnemyEntity()
	{
		return null;
	}

	public virtual float GetMoveSpeed()
	{
		return 0f;
	}

	public virtual float GetRotationSpeed()
	{
		return 5f;
	}
		
	public virtual float GetColliderRadius() {
		return 0f;
	}

	public virtual void SetAnimation(EAnimation anim) {
		
	}

	public virtual float GetDetectRange() {
		return 0f;
	}

	public virtual int GetHealth ()
	{
		return 0;
	}

	public virtual void SetHealth (int value)
	{
		if (OnHealthChange != null)
		{
			var health = GetHealth();
			OnHealthChange(health, value > GetMaxHealth() ? GetMaxHealth() : value, GetMaxHealth());
		}
	}

	public virtual int GetMaxHealth() {
		return 0;
	}

	public virtual float GetAttackRange()
	{
		return 0f;
	}

	public virtual int GetAttackDamage()
	{
		return 0;
	}

	public virtual float GetAttackSpeed()
	{
		return 0f;
	}

	public virtual Vector3 GetTargetPosition()
	{
		return Vector3.zero;
	}

	public virtual void SetTargetPosition(Vector3 position)
	{
		
	}

	public virtual void SetCommand(LCEnum.ECommand value)
	{
		
	}

	public virtual LCEnum.ECommand GetCommand()
	{
		return LCEnum.ECommand.AI;
	}

	public virtual void SetOwner(LCEntity owner)
	{
		m_Owner = owner;
	}

	public virtual LCEntity GetOwner()
	{
		return m_Owner;
	}

	public virtual LCEntity GetAttacker()
	{
		return null;
	}

	public virtual void SetAttacker(LCEntity value)
	{
		
	}

	public virtual string GetName()
	{
		return string.Empty;
	}

	public virtual void SetName(string value)
	{
		
	}

	public virtual int[] GetDropResources()
	{
		return null;
	}

	public virtual int[] GetCostResources()
	{
		return null;
	}

	public virtual int[] GetResources()
	{
		return null;
	}

	#endregion

	#region Castle

	public virtual int GetWaveIndex()
	{
		return 0;
	}

	public virtual int GetTroopCount()
	{
		return 0;
	}

	public virtual Queue<LCEnum.EEntityType> GetTroopQueue()
	{
		return null;
	}

	public virtual void SetTroopQueue(params int[] values)
	{
		
	}

	public virtual void SetSingleTroop(int troop)
	{

	}

	public virtual int GetResourceBonus()
	{
		return 0;
	}

	public virtual int GetHealthPointBonus()
	{
		return 0;
	}

	public virtual float GetMoveSpeedBonus()
	{
		return 0;
	}

	public virtual int GetCereal()
	{
		return 0;
	}

	public virtual void SetCereal(int value)
	{
		
	}

	public virtual int GetFish()
	{
		return 0;
	}

	public virtual void SetFish(int value)
	{
		
	}

	public virtual int GetMeat()
	{
		return 0;
	}

	public virtual void SetMeat(int value)
	{
		
	}

	public virtual int GetMetal()
	{
		return 0;
	}

	public virtual void SetMetal(int value)
	{
		
	}

	public virtual int GetPopulation()
	{
		return 0;
	}

	public virtual void SetPopulation(int value)
	{
		
	}

	public virtual int[] GetResourceCollections()
	{
		return null;
	}

	public virtual float GetTimeCollect()
	{
		return 0f;
	}

	public virtual float GetWaveCountDown()
	{
		return 0f;
	}

	public virtual void AddTroopWave()
	{
		
	}

	public virtual LCEnum.EEntityType[] GetTroopTypes()
	{
		return null;
	}

	public virtual int GetGoldWinBonus()
	{
		return 0;
	}

	#endregion

	#region Cave

	public virtual LCEnum.EEntityType GetMemberType()
	{
		return LCEnum.EEntityType.None;
	}

	public virtual void SetCurrentMember(int value)
	{
		
	}

	public virtual int GetCurrentMember()
	{
		return 0;
	}

	public virtual int GetMaxMember()
	{
		return 0;
	}

	public virtual float GetTimeSpawnSodier()
	{
		return 0f;
	}

	#endregion

	#region Lord

	public virtual int GetGold()
	{
		return 0;
	}

	public virtual void SetGold(int value)
	{
		if (OnGoldChange != null)
		{
			var gold = GetGold();
			OnGoldChange(gold, value > 999999 ? 999999 : value, 999999);
		}
	}

	public virtual List<LCEnum.EEntityType> GetMapUnlock()
	{
		return null;
	}

	public virtual void SetMapUnlock(LCEnum.EEntityType value)
	{

	}

	public virtual List<LCEnum.EEntityType> GetCastleUnlock()
	{
		return null;
	}

	public virtual void SetCastleUnlock(LCEnum.EEntityType value)
	{

	}

	#endregion

}

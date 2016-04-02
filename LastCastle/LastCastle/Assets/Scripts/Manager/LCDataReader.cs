using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;
using System.Linq;

public class LCDataReader : CMonoBehaviour
{

	#region Singleton

	private static object m_SingletonLock = new object();
	private static LCDataReader m_Instance = null;

	public static LCDataReader Instance
	{
		get
		{
			lock (m_SingletonLock)
			{
				if (m_Instance == null)
				{
					var gameObject = GameObject.Instantiate(Resources.Load("Prefabs/DataReader")) as GameObject;
					m_Instance = gameObject.GetComponent<LCDataReader>();
				}
				return m_Instance;
			}
		}
	}

	public static LCDataReader GetInstance()
	{
		return Instance;
	}

	#endregion

	#region Property

	private static Dictionary<LCEnum.EEntityType, LCBaseData> m_ListEntitiesData= new Dictionary<LCEnum.EEntityType, LCBaseData> ();
	private static List<LCCastleData> m_CastleDatas = new List<LCCastleData>();
	private static List<ObjectPoolData> m_ListObjectPoolData = new List<ObjectPoolData>();
	private static List<LCMapObject> m_WorldMapObjects = new List<LCMapObject> ();

	private static bool m_AlreadyInit = false;
	private static int m_MaxPoolAmount;
	public int MaxPoolAmount{
		get { return m_MaxPoolAmount; }
	}

	#endregion

	#region Implementation Monobehaviour

	void Awake()
	{
		m_Instance = this;
		DontDestroyOnLoad(this);
		if (m_AlreadyInit == false)
		{
			LoadData();
		}
	}

	#endregion

	#region Main methods

	private void LoadData() {
		var humanAsset = Resources.Load<TextAsset>("Data/Human");
		var demonAsset = Resources.Load<TextAsset>("Data/Demon");
		var castleAsset = Resources.Load<TextAsset>("Data/Castle");
		var caveAsset = Resources.Load<TextAsset>("Data/Cave");
		var animalAsset = Resources.Load<TextAsset>("Data/Animal");
		var castleResourceAsset = Resources.Load<TextAsset>("Data/CastleResource");
		var poolTextAsset = Resources.Load<TextAsset> ("Data/ObjectPool");
		var mapAsset = Resources.Load<TextAsset>("Data/MapObject");
		var worldMapAsset = Resources.Load<TextAsset>("Data/WorldMap");
		var lordAssetText = PlayerPrefs.GetString(Utilities.LORD_DATA, string.Empty);
		if (string.IsNullOrEmpty(lordAssetText))
		{
			lordAssetText = Resources.Load<TextAsset>("Data/Lord").text;
		}

		var jsonHuman = Json.Deserialize (humanAsset.text) as Dictionary<string, object>;
		var jsonDemon = Json.Deserialize (demonAsset.text) as Dictionary<string, object>;
		var jsonCastle = Json.Deserialize (castleAsset.text) as Dictionary<string, object>;
		var jsonCave = Json.Deserialize (caveAsset.text) as Dictionary<string, object>;
		var jsonAnimal = Json.Deserialize (animalAsset.text) as Dictionary<string, object>;
		var jsonCastleResource = Json.Deserialize (castleResourceAsset.text) as Dictionary<string, object>;
		var jsonPool = Json.Deserialize(poolTextAsset.text) as Dictionary<string, object>;
		var jsonMap = Json.Deserialize(mapAsset.text) as Dictionary<string, object>;
		var jsonWorldMap = Json.Deserialize (worldMapAsset.text) as Dictionary<string, object>;
		var jsonLord = Json.Deserialize (lordAssetText) as Dictionary<string, object>;

		LoadSodier(jsonHuman["humans"] as List<object>);
		LoadSodier(jsonDemon["demons"] as List<object>);
		LoadCastle(jsonCastle["castles"] as List<object>);
		LoadCave(jsonCave["caves"] as List<object>);
		LoadAnimal(jsonAnimal["animals"] as List<object>);
		LoadCastleResource(jsonCastleResource["castleResources"] as List<object>);
		LoadObjectPool(jsonPool as Dictionary<string, object>);
		LoadMap(jsonMap["maps"] as List<object>);
		LoadWorldMap(jsonWorldMap["maps"] as List<object>);
		LoadLord(jsonLord as Dictionary<string, object>);

		m_AlreadyInit = true;
	}

	private void LoadSodier(List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values[i] as Dictionary<string, object>;
			var sodier = new LCSodierData ();
			sodier.ID = int.Parse (instance["ID"].ToString());
			sodier.Name = instance["Name"].ToString();
			sodier.Description = instance["Description"].ToString();
			sodier.ModelPaths = ConvertTo<string> (instance["ModelPaths"] as List<object>);
			sodier.FSMPath = instance["FSMPath"].ToString ();
			sodier.Icon = instance["Icon"].ToString();
			sodier.EntityType = (LCEnum.EEntityType)int.Parse (instance["EntityType"].ToString());
			sodier.CurrentHealthPoint = int.Parse (instance["CurrentHealthPoint"].ToString());
			sodier.MaxHealthPoint = int.Parse (instance["MaxHealthPoint"].ToString());
			sodier.AttackDamage = int.Parse (instance["AttackDamage"].ToString());
			sodier.AttackRange = float.Parse (instance["AttackRange"].ToString());
			sodier.AttackSpeed = float.Parse (instance["AttackSpeed"].ToString());
			sodier.DetectRange = float.Parse (instance["DetectRange"].ToString());
			sodier.MoveSpeed = float.Parse (instance["MoveSpeed"].ToString());
			sodier.CostResources = ConvertTo<int>(instance["CostResources"] as List<object>);
			sodier.Population = int.Parse(instance["Population"].ToString());
			sodier.DropResources = ConvertTo<int> (instance["DropResources"] as List<object>);
			m_ListEntitiesData.Add (sodier.EntityType, sodier);
		}
	}

	private void LoadCastle(List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values[i] as Dictionary<string, object>;
			var castle = new LCCastleData ();
			castle.ID = int.Parse (instance["ID"].ToString());
			castle.Name = instance["Name"].ToString();
			castle.Description = instance["Description"].ToString();
			castle.ModelPaths = ConvertTo<string> (instance["ModelPaths"] as List<object>);
			castle.FSMPath = instance["FSMPath"].ToString ();
			castle.Icon = instance["Icon"].ToString();
			castle.EntityType = (LCEnum.EEntityType)int.Parse (instance["EntityType"].ToString());
			castle.CurrentHealthPoint = int.Parse (instance["CurrentHealthPoint"].ToString());
			castle.MaxHealthPoint = int.Parse (instance["MaxHealthPoint"].ToString());
			castle.Resources = ConvertTo<int> (instance["Resources"] as List<object>);
			castle.TroopTypes = ConvertToEnumEntityType(instance["TroopTypes"] as List<object>);
			castle.Population = int.Parse(instance["Population"].ToString());
			castle.ResourceBonus = int.Parse (instance["ResourceBonus"].ToString());
			castle.HealthPointBonus = int.Parse (instance["HealthPointBonus"].ToString());
			castle.MoveSpeedBonus = float.Parse (instance["MoveSpeedBonus"].ToString());
			castle.GoldWinBonus = int.Parse(instance["GoldWinBonus"].ToString());
			castle.CostGold = int.Parse(instance["CostGold"].ToString());
			var waveAsset = Resources.Load<TextAsset> (instance["WavePath"].ToString());
			var jsonWave = Json.Deserialize(waveAsset.text) as Dictionary<string, object>;
			castle.ListWave = new List<LCWaveData>();
			LoadWave(jsonWave["troopWaves"] as List<object>, castle.ListWave); 
			m_ListEntitiesData.Add (castle.EntityType, castle);
			m_CastleDatas.Add(LCCastleData.Clone(castle));
		}
	}

	private void LoadCave(List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values[i] as Dictionary<string, object>;
			var cave = new LCCaveData ();
			cave.ID = int.Parse (instance["ID"].ToString());
			cave.Name = instance["Name"].ToString();
			cave.Description = instance["Description"].ToString();
			cave.ModelPaths = ConvertTo<string> (instance["ModelPaths"] as List<object>);
			cave.FSMPath = instance["FSMPath"].ToString ();
			cave.Icon = instance["Icon"].ToString();
			cave.EntityType = (LCEnum.EEntityType)int.Parse (instance["EntityType"].ToString());
			cave.MemberType = (LCEnum.EEntityType)int.Parse (instance["MemberType"].ToString());
			cave.MaxMember = int.Parse (instance["MaxMember"].ToString());
			cave.TimeSpawnSodier = float.Parse (instance["TimeSpawnSodier"].ToString());
			m_ListEntitiesData.Add (cave.EntityType, cave);
		}
	}

	private void LoadAnimal(List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values[i] as Dictionary<string, object>;
			var animal = new LCAnimalData ();
			animal.ID = int.Parse (instance["ID"].ToString());
			animal.Name = instance["Name"].ToString();
			animal.Description = instance["Description"].ToString();
			animal.ModelPaths = ConvertTo<string> (instance["ModelPaths"] as List<object>);
			animal.FSMPath = instance["FSMPath"].ToString ();
			animal.Icon = instance["Icon"].ToString();
			animal.EntityType = (LCEnum.EEntityType)int.Parse (instance["EntityType"].ToString());
			animal.CurrentHealthPoint = int.Parse (instance["CurrentHealthPoint"].ToString());
			animal.MaxHealthPoint = int.Parse (instance["MaxHealthPoint"].ToString());
			animal.AttackDamage = int.Parse (instance["AttackDamage"].ToString());
			animal.AttackRange = float.Parse (instance["AttackRange"].ToString());
			animal.AttackSpeed = float.Parse (instance["AttackSpeed"].ToString());
			animal.DetectRange = float.Parse (instance["DetectRange"].ToString());
			animal.MoveSpeed = float.Parse (instance["MoveSpeed"].ToString());
			animal.DropResources = ConvertTo<int> (instance["DropResources"] as List<object>);
			m_ListEntitiesData.Add (animal.EntityType, animal);
		}
	}


	private void LoadCastleResource(List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values[i] as Dictionary<string, object>;
			var castle = new LCCastleResourceData ();
			castle.ID = int.Parse (instance["ID"].ToString());
			castle.Name = instance["Name"].ToString();
			castle.Description = instance["Description"].ToString();
			castle.ModelPaths = ConvertTo<string> (instance["ModelPaths"] as List<object>);
			castle.FSMPath = instance["FSMPath"].ToString ();
			castle.Icon = instance["Icon"].ToString();
			castle.EntityType = (LCEnum.EEntityType)int.Parse (instance["EntityType"].ToString());
			castle.CurrentHealthPoint = 9999;
			castle.MaxHealthPoint = 9999;
			castle.ResourceCollects = ConvertTo<int>(instance["ResourceCollects"] as List<object>);
			castle.TimeCollect = float.Parse (instance["TimeCollect"].ToString());
			m_ListEntitiesData.Add (castle.EntityType, castle);
		}
	}

	private void LoadObjectPool(Dictionary<string, object> data) {
		foreach (var item in data)
		{
			var obj = new ObjectPoolData();
			obj.GameType = (LCEnum.EEntityType) Enum.Parse(typeof (LCEnum.EEntityType), item.Key.ToString());
			obj.Amount = int.Parse(item.Value.ToString());
			m_MaxPoolAmount += obj.Amount;
			m_ListObjectPoolData.Add(obj);
		}
	}

	private void LoadWave(List<object> values, List<LCWaveData> waveData) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values[i] as Dictionary<string, object>;
			var wave = new LCWaveData ();
			wave.ID = int.Parse(instance["ID"].ToString());
			wave.TimeDelay = int.Parse (instance["TimeDelay"].ToString());
			wave.WaveDatas = ConvertStringToInts(instance["WaveDatas"] as List<object>);
			waveData.Add(wave);
		}
	}

	private void LoadMap(List<object> values)
	{
		for (int i = 0; i < values.Count; i++)
		{
			var instance = values[i] as Dictionary<string, object>;
			var obj = new LCMapObject();
			obj.MapObject = (LCEnum.EEntityType)int.Parse(instance["MapObject"].ToString());
			obj.Position = Utilities.ConvertStringToV3(instance["Position"].ToString());
			obj.Rotation = Utilities.ConvertStringToQuaternion(instance["Rotation"].ToString());
			obj.ObjectType = (LCEnum.EObjectType)int.Parse(instance["ObjectType"].ToString());
			obj.Team = (LCEnum.EClass)int.Parse(instance["Team"].ToString());
			obj.UnlockMap = (LCEnum.EEntityType)int.Parse(instance["UnlockMap"].ToString());
			LoadMapObject(instance["Childs"] as List<object>, obj);
			m_ListEntitiesData[obj.MapObject] = obj;
		}
	}

	private void LoadMapObject(List<object> values, LCMapObject parent)
	{
		parent.Childs = new LCMapObject[values.Count];
		for (int i = 0; i < values.Count; i++)
		{
			var instance = values[i] as Dictionary<string, object>;
			var obj = new LCMapObject();
			obj.MapObject = (LCEnum.EEntityType)int.Parse(instance["MapObject"].ToString());
			obj.Position = Utilities.ConvertStringToV3(instance["Position"].ToString());
			obj.Rotation = Utilities.ConvertStringToQuaternion(instance["Rotation"].ToString());
			obj.ObjectType = (LCEnum.EObjectType)int.Parse(instance["ObjectType"].ToString());
			obj.Team = (LCEnum.EClass)int.Parse(instance["Team"].ToString());
			LoadMapObject(instance["Childs"] as List<object>, obj);
			parent.Childs[i] = obj;
		}
	}

	private void LoadWorldMap(List<object> mapValues)
	{
		for (int i = 0; i < mapValues.Count; i++)
		{
			var instance = mapValues[i] as Dictionary<string, object>;
			var obj = new LCMapObject();
			obj.Icon = instance["Icon"].ToString();
			obj.MapObject = (LCEnum.EEntityType)int.Parse(instance["MapObject"].ToString());
			obj.Position = Utilities.ConvertStringToV3(instance["Position"].ToString());
			m_WorldMapObjects.Add(obj);
		}
	}

	private void LoadLord(Dictionary<string, object> instance)
	{
		var lord = new LCLordData();
		lord.ID = int.Parse(instance["ID"].ToString());
		lord.Name = instance["Name"].ToString();
		lord.Icon = instance["Icon"].ToString();
		lord.ModelPaths = ConvertTo<string> (instance["ModelPaths"] as List<object>);
		lord.EntityType = (LCEnum.EEntityType)int.Parse(instance["EntityType"].ToString());
		lord.TimeCreate = instance["TimeCreate"].ToString();
		lord.Gold = int.Parse(instance["Gold"].ToString());
		var mapUnlocks = ConvertToEnumEntityType (instance["MapUnlock"] as List<object>);
		lord.MapUnlock = new List<LCEnum.EEntityType>(mapUnlocks);
		var castleUnlocks = ConvertToEnumEntityType (instance["CastleUnlock"] as List<object>);
		lord.CastleUnlock = new List<LCEnum.EEntityType>(castleUnlocks);
		m_ListEntitiesData[lord.EntityType] = lord;
	}

	#endregion

	#region Getter 

	public LCLordData GetLordData()
	{
		return LCLordData.Clone(m_ListEntitiesData[LCEnum.EEntityType.Lord] as LCLordData);
	}

	public List<LCMapObject> GetWorldMapDatas() {
		return m_WorldMapObjects;
	}

	public LCMapObject GetMapData(LCEnum.EEntityType type) {
		return m_ListEntitiesData[type] as LCMapObject;
	}

	public void GetDefaultValue(ref Dictionary<LCEnum.EEntityType, LCBaseData> defaultValue)
	{
		foreach (var item in m_ListEntitiesData)
		{
			defaultValue[item.Key] = m_ListEntitiesData[item.Key];
		}
	}

	public LCCastleData GetCastleData(LCEnum.EEntityType type) {
		return LCCastleData.Clone(m_ListEntitiesData[type] as LCCastleData);
	}

	public List<LCCastleData> GetCastleData() {
		return m_CastleDatas;
	}

	public LCCaveData GetCaveData(LCEnum.EEntityType type) {
		return LCCaveData.Clone(m_ListEntitiesData[type] as LCCaveData);
	}

	public LCCastleResourceData GetCastleResourceData(LCEnum.EEntityType type) {
		return LCCastleResourceData.Clone(m_ListEntitiesData[type] as LCCastleResourceData);
	}

	public LCSodierData GetSodierData(LCEnum.EEntityType type) {
		return LCSodierData.Clone(m_ListEntitiesData[type] as LCSodierData);
	}

	public LCAnimalData GetAnimalData(LCEnum.EEntityType type) {
		return LCAnimalData.Clone(m_ListEntitiesData[type] as LCAnimalData);
	}

	public List<ObjectPoolData> GetObjectPoolData() {
		return m_ListObjectPoolData;
	}

	public ObjectPoolData GetObjectPoolData(LCEnum.EEntityType type) {
		for (int i = 0; i < m_ListObjectPoolData.Count; i++)
		{
			if (m_ListObjectPoolData[i].GameType == type)
			{
				return m_ListObjectPoolData[i];
			}
		}
		return null;
	}

	#endregion

	#region Utilities

	private LCEnum.EResourcesType[] ConvertToEnumResource(List<object> instance) {
		var results = new LCEnum.EResourcesType[instance.Count];
		for (int i = 0; i < instance.Count; i++) {
			var typeEnum = (LCEnum.EResourcesType) int.Parse (instance[i].ToString());
			results[i] = typeEnum;
		}
		return results;
	}

	private LCEnum.EEntityType[] ConvertToEnumEntityType(List<object> instance) {
		var results = new LCEnum.EEntityType[instance.Count];
		for (int i = 0; i < instance.Count; i++) {
			var typeEnum = (LCEnum.EEntityType) int.Parse (instance[i].ToString());
			results[i] = typeEnum;
		}
		return results;
	}

	private List<int[]> ConvertStringToInts (List<object> instance) {
		var result = new List<int[]>();
		for (int i = 0; i < instance.Count; i++) {
			var strs = instance[i].ToString().Split(',');
			var indexs = new int[strs.Length];
			for (int x = 0; x < strs.Length; x++) {
				indexs[x] = int.Parse(strs[x].ToString());
			}
			result.Add(indexs);
		}
		return result;
	}

	private T[] ConvertTo<T> (List<object> instance) {
		var result = new T[instance.Count];
		for (int i = 0; i < instance.Count; i++) {
			result[i] = (T)Convert.ChangeType (instance[i], typeof (T));
		}
		return result;
	}

	private LCEnum.EEntityType[] ConvertIntToEnums (List<object> instance) {
		var result = new LCEnum.EEntityType[instance.Count];
		for (int i = 0; i < instance.Count; i++) {
			result[i] = (LCEnum.EEntityType)int.Parse (instance[i].ToString());
		}
		return result;
	}

	private LCEnum.EEntityType[] ConvertStringToEnums (List<object> instance) {
		var result = new LCEnum.EEntityType[instance.Count];
		for (int i = 0; i < instance.Count; i++) {
			result[i] = (LCEnum.EEntityType)Enum.Parse (typeof(LCEnum.EEntityType), instance[i].ToString());
		}
		return result;
	}

	#endregion

}











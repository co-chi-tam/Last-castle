using UnityEngine;
using System.Collections;

public class LCMapObject : LCBaseData
{

	private LCEnum.EEntityType m_MapObject;
	private Vector3 m_Position;
	private Quaternion m_Rotation;
	private LCEnum.EObjectType m_ObjectType;
	private LCEnum.EClass m_Team;
	private LCEnum.EEntityType m_UnlockMap;
	private LCMapObject[] m_Childs;

	public LCEnum.EEntityType MapObject
	{
		get { return m_MapObject; }
		set { m_MapObject = value; }
	}

	public Vector3 Position
	{
		get { return m_Position; }
		set { m_Position = value; }
	}

	public Quaternion Rotation
	{
		get { return m_Rotation; }
		set { m_Rotation = value; }
	}

	public LCEnum.EObjectType ObjectType
	{
		get { return m_ObjectType; }
		set { m_ObjectType = value; }
	}

	public LCEnum.EClass Team
	{
		get { return m_Team; }
		set { m_Team = value; }
	}

	public LCEnum.EEntityType UnlockMap
	{
		get { return m_UnlockMap; }
		set { m_UnlockMap = value; }
	}

	public LCMapObject[] Childs
	{
		get { return m_Childs; }
		set { m_Childs = value; }
	}

	public LCMapObject()
	{
		this.MapObject = LCEnum.EEntityType.None;
		this.Position = Vector3.zero;
		this.Rotation = Quaternion.identity;
		this.ObjectType = LCEnum.EObjectType.Neutral;
		this.Team = LCEnum.EClass.None;
		this.Childs = null;
		this.UnlockMap = LCEnum.EEntityType.None;
	}

}


using System;

public class ObjectPoolData
{
	private LCEnum.EEntityType m_GameType;
	private int m_Amount;

	public LCEnum.EEntityType GameType {
		get { return m_GameType; }
		set { m_GameType = value; }
	}

	public int Amount {
		get { return m_Amount; }
		set { m_Amount = value; }
	}

	public ObjectPoolData()
	{
		m_GameType = LCEnum.EEntityType.None;
		m_Amount = 0;
	}

}


using UnityEngine;
using System.Collections.Generic;

public class LCWaveData : LCInfo
{
	private float m_TimeDelay;
	private List<int[]> m_WaveDatas;

	public float TimeDelay
	{
		get { return m_TimeDelay; }
		set { m_TimeDelay = value; }
	}

	public List<int[]> WaveDatas
	{
		get { return m_WaveDatas; }
		set { m_WaveDatas = value; }
	}

	public LCWaveData()
	{
		this.TimeDelay = 0;
		this.WaveDatas = null;
	}

}


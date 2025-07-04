using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BoxProb", menuName = "Assets/Create New BoxProb")]
public class BoxProbSO : ScriptableObject
{
    public List<BoxProb> _boxProbs = new List<BoxProb>();
}

[System.Serializable]
public class BoxProb
{
    public int DayNum;
    public float BoxType1Prob;
    public float BoxType2Prob;
    public float BoxType3Prob;
}
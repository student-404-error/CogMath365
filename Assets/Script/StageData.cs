using System;
using UnityEngine;
[Serializable]
public class StageData
{
    public float PlayTime;
    public int PlayedRound;
    public float Accuracy;
    public float AverageSpeed;
    public int Score;
}

[Serializable]
public class ProblemData
{
    public StageData S01, S02, S03, S04, S05, S06, S07, S08, S09, S10, S11, S12, S13, S14;
}

[Serializable]
public class DateData
{
    public ProblemData Problem;
}
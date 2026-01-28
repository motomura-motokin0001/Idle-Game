using System;

[Serializable]
public class DefaultData 
{

    public string Player_Name;//ログイン時に必要
    public string Password;//ログイン時に必要
    
    public double TotalMass;//合計質量（合計スコア）
    public float TotalTime;//合計プレイ時間

    //=========================================================================
    /*惑星ID順に格納*/
    public double[] Levels;
    public double[] PressStages;
    //=========================================================================
}   

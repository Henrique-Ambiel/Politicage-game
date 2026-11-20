
using UnityEngine;

[System.Serializable]
public class CrisisOption
{
    public string optionText;

    public int sovereigntyChange;
    public int budgetChange;
    public int popularityChange;
}

[CreateAssetMenu(
    fileName = "NewCrisis",
    menuName = "Game/Crisis"
)]
public class CrisisData : ScriptableObject
{
    public string crisisTitle;

    [TextArea(3, 6)]
    public string crisisDescription;

    public CrisisOption optionA;
    public CrisisOption optionB;
}

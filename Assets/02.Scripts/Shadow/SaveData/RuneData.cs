using System;

public class RuneData : SaveDataBase
{
    private double GoldRuneFinishTime = 0;
    private double ExpRuneFinishTime = 0;
    private double StoneRuneFinishTime = 0;
    private double DamageRuneFinishTime = 0; // 데미지룬으로 할거면 스탯데이터로

    public override void ValidCheck()
    {
        GoldRuneRemainTime = GoldRuneRemainTime > 0 ? GoldRuneRemainTime : 0;
        SoulRuneRemainTime = SoulRuneRemainTime > 0 ? SoulRuneRemainTime : 0;
        ArtifactRuneRemainTime = ArtifactRuneRemainTime > 0 ? ArtifactRuneRemainTime : 0;
        SpeedRuneRemainTime = SpeedRuneRemainTime > 0 ? SpeedRuneRemainTime : 0;
    }

    public bool IsGoldRuneActivate
    {
        get { return GoldRuneFinishTime > DateTime.un; }
    }

    public bool IsSoulRuneActivate
    {
        get { return SoulRuneFinishTime > ServerManager.ServerUnixTimeInterpolation; }
    }

    public bool IsArtifactRuneActivate
    {
        get { return ArtifactRuneFinishTime > ServerManager.ServerUnixTimeInterpolation; }
    }

    public bool IsSpeedRuneActivate
    {
        get { return SpeedRuneFinishTime > ServerManager.ServerUnixTimeInterpolation; }
    }

    public bool IsRunePackagePurchased => m_RuneInfo.IsRunePackagePurchased;

    public int GetRuneBuffRemainTime(RuneBuffType type)
    {
        int remainTime = 0;
        switch (type)
        {
            case RuneBuffType.Gold:
                remainTime = (int) (GoldRuneFinishTime - ServerManager.ServerUnixTimeInterpolation);
                break;
            case RuneBuffType.Soul:
                remainTime = (int) (SoulRuneFinishTime - ServerManager.ServerUnixTimeInterpolation);
                break;
            case RuneBuffType.Artifact:
                remainTime = (int) (ArtifactRuneFinishTime - ServerManager.ServerUnixTimeInterpolation);
                break;
            case RuneBuffType.Speed:
                remainTime = (int) (SpeedRuneFinishTime - ServerManager.ServerUnixTimeInterpolation);
                break;
        }

        return Mathf.Max(0, remainTime);
    }

    private bool IsADCountLimit(RuneBuffType type)
    {
        switch (type)
        {
            case RuneBuffType.Gold:
                return m_RuneInfo.GoldRuneDailyLimit <= 0;

            case RuneBuffType.Soul:
                return m_RuneInfo.SoulRuneDailyLimit <= 0;

            case RuneBuffType.Artifact:
                return m_RuneInfo.ArtifactRuneDailyLimit <= 0;

            case RuneBuffType.Speed:
                return m_RuneInfo.SpeedRuneDailyLimit <= 0;
        }

        return false;
    }

    private void StartRuneBuffCount(RuneBuffType type)
    {
        double buffFinishTime = GetRuneBuffRemainTime(type) + ServerManager.ServerUnixTimeInterpolation +
                                SystemValue.RUNE_BUFF_DURATION;

        switch (type)
        {
            case RuneBuffType.Gold:
                GoldRuneFinishTime = buffFinishTime;
                m_RuneInfo.GoldRuneDailyLimit -= 1;
                break;

            case RuneBuffType.Soul:
                SoulRuneFinishTime = buffFinishTime;
                m_RuneInfo.SoulRuneDailyLimit -= 1;
                break;

            case RuneBuffType.Artifact:
                ArtifactRuneFinishTime = buffFinishTime;
                m_RuneInfo.ArtifactRuneDailyLimit -= 1;
                break;

            case RuneBuffType.Speed:
                SpeedRuneFinishTime = buffFinishTime;
                m_RuneInfo.SpeedRuneDailyLimit -= 1;

                MainScene.Instance.SetTimeScale();
                break;
        }

        m_Dirty = true;

        RefreshEvent.Trigger(RefreshEventType.Rune);
        AchievementEvent.Trigger(AchievementType.DAILY_GetRuneBuff);

        Save(true);
    }

    private void CheckNextDay()
    {
        if (m_RuneInfo == null)
        {
            return;
        }

        // 보상을 다 안받아도 강제로 초기화
        DateTime today = ServerManager.ServerDateTimeToday;

        var diff = today - m_RuneInfo.LastSaveDateTime;
        if (diff.TotalDays >= 1)
        {
            m_RuneInfo.LastSaveDateTime = today;

            int limitCount = SystemValue.RUNE_DAILY_LIMIT;

            m_RuneInfo.GoldRuneDailyLimit = limitCount;
            m_RuneInfo.SoulRuneDailyLimit = limitCount;
            m_RuneInfo.ArtifactRuneDailyLimit = limitCount;
            m_RuneInfo.SpeedRuneDailyLimit = limitCount;

            m_Dirty = true;
        }
    }

    public int GetRuneBuffLimitCount(RuneBuffType type)
    {
        switch (type)
        {
            case RuneBuffType.Gold:
                return m_RuneInfo.GoldRuneDailyLimit;
            case RuneBuffType.Soul:
                return m_RuneInfo.SoulRuneDailyLimit;
            case RuneBuffType.Artifact:
                return m_RuneInfo.ArtifactRuneDailyLimit;
            case RuneBuffType.Speed:
                return m_RuneInfo.SpeedRuneDailyLimit;
        }

        return -1;
    }

    public void TryStartRuneBuff(RuneBuffType type)
    {
        if (IsADCountLimit(type))
        {
            ToastMessagePool.Instance.Show(LocalizeText.GetText("UI_Toast_DailyADReward_Disable"));
            return;
        }

        if (IsAlreadyActivate(type))
        {
            ToastMessagePool.Instance.Show(LocalizeText.GetText("UI_Toast_Rune_Already_Activate"));
            return;
        }

        if (m_RuneInfo.IsRunePackagePurchased)
        {
            StartRuneBuffCount(type);
        }
        else
        {
            AdManager.Instance.TryShowRequest(ADType.RuneBuff, () => { StartRuneBuffCount(type); });
        }
    }

    private bool IsAlreadyActivate(RuneBuffType type)
    {
        switch (type)
        {
            case RuneBuffType.Gold:
                return (!m_RuneInfo.IsRunePackagePurchased && IsGoldRuneActivate);

            case RuneBuffType.Soul:
                return (!m_RuneInfo.IsRunePackagePurchased && IsSoulRuneActivate);

            case RuneBuffType.Artifact:
                return (!m_RuneInfo.IsRunePackagePurchased && IsArtifactRuneActivate);

            case RuneBuffType.Speed:
                return (!m_RuneInfo.IsRunePackagePurchased && IsSpeedRuneActivate);
        }

        return false;
    }
}

public enum RuneBuffType
{
    Gold,
    Soul,
    Artifact,
    Speed,
}
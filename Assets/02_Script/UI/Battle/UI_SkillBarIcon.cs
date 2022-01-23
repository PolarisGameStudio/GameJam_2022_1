using UnityEngine;
using UnityEngine.UI;

public class UI_SkillBarIcon : MonoBehaviour
{
    [SerializeField] private Image _imgFrameIcon;
    [SerializeField] private Image _imgSkillIcon;
    [SerializeField] private Image _imgSkillCooldown;
    [SerializeField] private Text _txtSkillCooldown;

    [SerializeField] private GameObject _onSkillEmpty;
    [SerializeField] private GameObject _onSkillLock;

    private PlayerSkill _skill;
    private bool _isUnlock;

    public void InitSkill(PlayerSkill skill, bool isUnlock)
    {
        if (_skill != null && _skill == skill)
        {
            return;
        }

        _skill = skill;
        _isUnlock = isUnlock;

        Refresh();
    }

    private void Refresh()
    {
        if (!_isUnlock)
        {
            _onSkillLock.SetActive(true);
            return;
        }

        if (_skill == null)
        {
            _onSkillEmpty.SetActive(true);
            return;
        }

        _onSkillEmpty.SetActive(false);
        _onSkillLock.SetActive(false);

        _imgSkillIcon.sprite = AssetManager.Instance.SkillIcon[_skill.Data.Index];
        _imgFrameIcon.enabled = AssetManager.Instance.ItemFrameIcon[(int) _skill.Data.ItemGrade];
    }

    private void Update()
    {
        if (_skill == null)
        {
            return;
        }

        var coolTime = _skill.RemainCoolTime;

        if (coolTime <= 0)
        {
            _txtSkillCooldown.text = "";
            _imgSkillCooldown.fillAmount = 0;
        }
        else
        {
            _txtSkillCooldown.text = $"{_skill.RemainCoolTime:N1}";
            _imgSkillCooldown.fillAmount = 1 - _skill.RemainCoolTimeNormalized;
        }

        // _imgSkillCooldown.enabled = isSkillDisable;
        //  _txtSkillCooldown.enabled = isSkillDisable;
        //
        //  if (isSkillDisable)
        //  {
        //      _imgSkillCooldown.fillAmount = 1 - _skill.RemainCoolTimeNormalized;
        //      _txtSkillCooldown.text = $"{_skill.RemainCoolTime:N1}";
        //  }
        // _imgSkillCooldown.fillAmount = 1 - _skill.RemainCoolTimeNormalized;
    }

    // public void OnClickSlot()
    // {
    //     _skill.TryUseSkill();
    // }
}
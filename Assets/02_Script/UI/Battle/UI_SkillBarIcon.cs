
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillBarIcon : MonoBehaviour
{
    [SerializeField] private Image _imgSkillIcon;
    [SerializeField] private Image _imgSkillCooldown;
    [SerializeField] private Text _txtSkillCooldown;

    [SerializeField] private Transform _onSkillEmpty;
    [SerializeField] private Transform _onSkillLock;
    
    private PlayerSkill _skill;

    public void InitSkill(PlayerSkill skill)
    {
        if (_skill == skill)
        {
            return;
        }
        
        _skill = skill;

        Refresh();
    }

    private void Refresh()
    {
        // todo: 리소스 나오면 추가
        //_imgSkillIcon.sprite = null;
    }

    private void Update()
    {
        if (_skill == null)
        {
            return;
        }

        var isSkillDisable = !_skill.IsSkillEnable();
        
        _imgSkillCooldown.enabled = isSkillDisable;
        _txtSkillCooldown.enabled = isSkillDisable;

        if (isSkillDisable)
        {
            _imgSkillCooldown.fillAmount = 1 - _skill.RemainCoolTimeNormalized;
            _txtSkillCooldown.text = $"{_skill.RemainCoolTime:N1}";
        }
    }

    public void OnClickSlot()
    {
        _skill.TryUseSkill();
    }
}

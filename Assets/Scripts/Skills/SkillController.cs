using UnityEngine;

public class SkillController : MonoBehaviour
{
    private bool _isBlueSkill = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetSkill();
        }
    }

    private void SetSkill()
    {
        _isBlueSkill = !_isBlueSkill;

        if (_isBlueSkill) GameManager.Instance.Skill = SkillType.BlueProjectile;
        else GameManager.Instance.Skill = SkillType.YellowProjectile;
        UIManager.Instance.SetSkillUI();
    }
}

public enum SkillType
{
    BlueProjectile,
    YellowProjectile,
}

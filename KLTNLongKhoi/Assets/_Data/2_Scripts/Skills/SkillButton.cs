using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KLTNLongKhoi
{
    public class SkillButton : MonoBehaviour
    {
        [SerializeField] private string skillName;
        [TextArea(3, 10)][SerializeField] private string skillDescription;
        [SerializeField] private List<SkillUpgradeStats> upgradeStats;
        [SerializeField] private int skillLevel;
        [SerializeField] private Sprite skillIcon;
        private UISkill uiSkill;

        PlayerStatsManager playerStatsManager;

        public string SkillName { get => skillName; }
        public string SkillDescription { get => skillDescription; }
        public Sprite SkillIcon { get => skillIcon; }
        public int SkillLevel { get => skillLevel; set => skillLevel = value; }

        void OnEnable()
        {
            GetComponent<Button>().onClick.AddListener(OnClickSkill);
            uiSkill = GetComponentInParent<UISkill>();
            playerStatsManager = FindFirstObjectByType<PlayerStatsManager>();
            UpdateSkillLevel();
        }

        private void UpdateSkillLevel()
        {
            if (skillName == "C")
            {
                skillLevel = playerStatsManager.PlayerData.levelSkillC;
            }
            else if (skillName == "E")
            {
                skillLevel = playerStatsManager.PlayerData.levelSkillE;
            }
            else if (skillName == "Q")
            {
                skillLevel = playerStatsManager.PlayerData.levelSkillQ;
            }
        }

        private void OnClickSkill()
        {
            uiSkill.ShowSkillInfo(this);
        }

        public void OnClickUpgradeButton()
        {
            SkillUpgradeStats upgradeStat = null;
            skillLevel++;
            if (skillLevel > 0 && skillLevel <= upgradeStats.Count)
            {
                upgradeStat = upgradeStats[skillLevel - 1];
            }
            else
            {
                skillLevel = upgradeStats.Count;
                uiSkill.UpgradeFail("Max level reached!");
                return;
            }

            if (playerStatsManager.CurrentMoney < upgradeStat.cost)
            {
                Debug.Log("Not enough money!");
                uiSkill.UpgradeFail("Not enough money!");
                return;
            }
            else
            {
                playerStatsManager.CurrentMoney -= upgradeStat.cost;
                playerStatsManager.PlayerData.baseHP += upgradeStat.hpBonus;
                playerStatsManager.PlayerData.baseSP += upgradeStat.spBonus;
                playerStatsManager.PlayerData.baseMP += upgradeStat.mpBonus;
                playerStatsManager.PlayerData.baseStr += upgradeStat.physicalDamage;
                playerStatsManager.PlayerData.baseInt += upgradeStat.magicDamage;
                playerStatsManager.PlayerData.baseCri += upgradeStat.criticalChance;

                if (skillName == "C")
                {
                    playerStatsManager.PlayerData.levelSkillC = skillLevel;
                }
                else if (skillName == "E")
                {
                    playerStatsManager.PlayerData.levelSkillE = skillLevel;
                }
                else if (skillName == "Q")
                {
                    playerStatsManager.PlayerData.levelSkillQ = skillLevel;
                }
 
                uiSkill.UpgradeSuccess("Upgrade success!");
            }

        }
    }
}

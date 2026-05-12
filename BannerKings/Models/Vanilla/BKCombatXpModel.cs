using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using BannerKings.Managers.Education;
using BannerKings.Managers.Education.Lifestyles;

namespace BannerKings.Models.Vanilla
{
    public class BKCombatXpModel : DefaultCombatXpModel
    {

        public override ExplainedNumber GetXpFromHit(CharacterObject attackerTroop, CharacterObject captain, CharacterObject attackedTroop, 
            PartyBase attackerParty, int damage, bool isFatal, CombatXpModel.MissionTypeEnum missionType)
        {
            var xpAmount = base.GetXpFromHit(attackerTroop, captain, attackedTroop, attackerParty, damage, isFatal, missionType);
            var hero = attackedTroop.HeroObject;
            if (hero != null && missionType == MissionTypeEnum.Tournament)
            {
                var data = BannerKingsConfig.Instance.EducationManager.GetHeroEducation(hero);
                if (data.Lifestyle != null && data.Lifestyle.Equals(DefaultLifestyles.Instance.Gladiator))
                {
                    xpAmount.AddFactor(2f, DefaultLifestyles.Instance.Gladiator.Name);
                }
            }

            return xpAmount;
        }
    }
}

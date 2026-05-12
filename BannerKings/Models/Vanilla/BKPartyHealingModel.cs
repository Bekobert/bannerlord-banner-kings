using System;
using BannerKings.Managers.Court.Members.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace BannerKings.Models.Vanilla
{
    public class BKPartyHealingModel : DefaultPartyHealingModel
    {
        private static readonly TextObject _starvingText = new TextObject("{=jZYUdkXF}Starving");
        public override ExplainedNumber GetDailyHealingForRegulars(PartyBase party, bool isPrisoners, bool includeDescriptions = false)
        {
            ExplainedNumber bonuses = base.GetDailyHealingForRegulars(party, isPrisoners, includeDescriptions);
            MobileParty mobileParty = party.MobileParty;
            Boolean isInBesiegedStarvingCity = mobileParty.CurrentSettlement != null && mobileParty.CurrentSettlement.IsUnderSiege && mobileParty.CurrentSettlement.IsStarving;
            if (isInBesiegedStarvingCity && !mobileParty.IsGarrison)
            {
                int num = MBRandom.RoundRandomized((float)mobileParty.MemberRoster.TotalRegulars * 0.1f);
                bonuses.Add(-num, _starvingText);
            }
            return bonuses;
        }

        public override ExplainedNumber GetDailyHealingHpForHeroes(PartyBase party, bool isPrisoners, bool includeDescriptions = false)
        {
            ExplainedNumber result = base.GetDailyHealingHpForHeroes(party, includeDescriptions);
            Hero leader = party.LeaderHero;
            MobileParty mobileParty = party.MobileParty;
            if (leader != null && mobileParty.CurrentSettlement != null)
            {
                if (BannerKingsConfig.Instance.CourtManager.HasCurrentTask(leader.Clan, DefaultCouncilTasks.Instance.FamilyCare,
                    out float healCompetence))
                {
                    result.AddFactor(0.2f * healCompetence, DefaultCouncilTasks.Instance.FamilyCare.Name);
                }
            }

            return result;
        }
    }
}

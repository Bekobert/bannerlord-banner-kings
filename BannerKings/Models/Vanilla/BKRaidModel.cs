using BannerKings.Managers.Populations.Villages;
using BannerKings.Managers.Skills;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.MapEvents;

namespace BannerKings.Models.Vanilla
{
    public class BKRaidModel : DefaultRaidModel
    {
        public override ExplainedNumber CalculateHitDamage(MapEventSide attackerSide, float settlementHitPoints)
        {
            var result = base.CalculateHitDamage(attackerSide, settlementHitPoints);
            var attacker = attackerSide.LeaderParty;
            if (attacker is {LeaderHero: { }})
            {
                var reference = result;
                var education = BannerKingsConfig.Instance.EducationManager.GetHeroEducation(attacker.LeaderHero);
                if (education.HasPerk(BKPerks.Instance.OutlawPlunderer))
                {
                    result.Add(reference.ResultNumber * 1.15f - reference.ResultNumber, BKPerks.Instance.OutlawPlunderer.Name);
                }

                if (education.HasPerk(BKPerks.Instance.MercenaryRansacker))
                {
                    result.Add(reference.ResultNumber * 1.15f - reference.ResultNumber, BKPerks.Instance.MercenaryRansacker.Name);
                }

                if (education.HasPerk(BKPerks.Instance.VaryagShieldBrother))
                {
                    result.Add(reference.ResultNumber * 1.15f - reference.ResultNumber, BKPerks.Instance.VaryagShieldBrother.Name);
                }

                if (education.HasPerk(BKPerks.Instance.JawwalGhazw))
                {
                    result.Add(reference.ResultNumber * 1.15f - reference.ResultNumber, BKPerks.Instance.JawwalGhazw.Name);
                }

                if (education.HasPerk(BKPerks.Instance.KheshigRaider))
                {
                    result.Add(reference.ResultNumber * 1.15f - reference.ResultNumber, BKPerks.Instance.KheshigRaider.Name);
                }
            }

            var settlement = attackerSide.MapEvent.MapEventSettlement;
            if (settlement != null)
            {
                if (BannerKingsConfig.Instance.PopulationManager.IsSettlementPopulated(settlement))
                {
                    var data = BannerKingsConfig.Instance.PopulationManager.GetPopData(settlement).VillageData;
                    var palisade = data.GetBuildingLevel(DefaultVillageBuildings.Instance.Palisade);
                    if (palisade > 0)
                    {
                        result.AddFactor(-0.12f * palisade, DefaultVillageBuildings.Instance.Palisade.Name);
                    }
                }
            }

            return result;
        }
    }
}
using BannerKings.Settings;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;

namespace BannerKings.Models.Vanilla
{
    public class BKBanditModel : DefaultBanditDensityModel
    {
        public override int GetMaxSupportedNumberOfLootersForClan(Clan clan) 
        {
            if (clan.StringId == "looters")
                return BannerKingsSettings.Instance.BanditPartiesLimit;

            return base.GetMaxSupportedNumberOfLootersForClan(clan);
        }
        public override int NumberOfMaximumBanditPartiesAroundEachHideout => 20;
    }
}

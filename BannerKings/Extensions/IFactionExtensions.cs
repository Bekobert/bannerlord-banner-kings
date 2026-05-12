using Helpers;
using System.Linq;
using TaleWorlds.CampaignSystem;

namespace BannerKings.Extensions
{
    public static class IFactionExtensions
    {
        /*
        public static bool IsKingdomAtWar(this IFaction faction) => faction.Stances.Any(x => x.IsAtWar && 
        x.Faction1.IsKingdomFaction &&
        x.Faction2.IsKingdomFaction);
        */
        public static bool isKingdomAtWar(this IFaction faction) => FactionHelper.GetStances(faction).Any(x => x.IsAtWar && x.Faction1.IsKingdomFaction && x.Faction2.IsKingdomFaction);
    }
}

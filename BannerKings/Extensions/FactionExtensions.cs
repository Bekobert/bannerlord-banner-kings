using System.Collections.Generic;
using TaleWorlds.CampaignSystem;

namespace BannerKings.Extensions
{
    public static class FactionExtensions
    {
        public static List<IFaction> GetAllies(this IFaction faction)
        {
            List<IFaction> list = new List<IFaction>(3);
            /*foreach (var stance in faction.Stances)
            {
                if (stance.IsAllied)
                {
                    list.Add(stance.Faction1 == faction ? stance.Faction2 : stance.Faction1);
                }
            }*/

            if (faction is Kingdom kingdom)
            {
                foreach (var ally in kingdom.AlliedKingdoms)
                {
                    list.Add(ally);
                }

                return list;
            }

            if(faction is Clan clan && clan.Kingdom != null)
            {
                foreach (var ally in clan.Kingdom.AlliedKingdoms)
                {
                    list.Add(ally);
                }
                return list;
            }

            return list;
        }
    }
}

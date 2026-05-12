using BannerKings.Managers.Populations.Estates;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace BannerKings.Components
{
    internal class EstateComponent : BannerKingsComponent
    {
        public EstateComponent(Settlement origin, Estate estate) : base(origin, 
            "{=NzSOneTv}Estate Retinue from {ORIGIN}")
        {
            Behavior = AiBehavior.Hold;
            Estate = estate;
        }

        [SaveableProperty(1001)] public MobileParty Escort { get; set; }
        [SaveableProperty(1002)] public AiBehavior Behavior { get; set; }
        [SaveableProperty(1003)] public Estate Estate { get; set; }

        public override TextObject Name => new TextObject("{=NzSOneTv}Estate Retinue from {ORIGIN}")
            .SetTextVariable("ORIGIN", HomeSettlement.Name);

        protected override void OnInitialize()
        {
            base.OnInitialize();
            MobileParty.SetPartyUsedByQuest(true);
            MobileParty.Party.SetVisualAsDirty();
            MobileParty.Ai.SetInitiative(0.5f, 1f, float.MaxValue);
            MobileParty.ShouldJoinPlayerBattles = true;
            MobileParty.Aggressiveness = 0.1f;
            MobileParty.SetWagePaymentLimit(Campaign.Current.Models.PartyWageModel.MaxWagePaymentLimit);
        }

        private static MobileParty CreateParty(string id, Estate estate, Settlement origin)
        {
            var component = new EstateComponent(origin, estate);
            return MobileParty.CreateParty(id, new EstateComponent(origin, estate));
        }

        public static void CreateRetinue(Estate estate)
        {
            Settlement origin = estate.EstatesData.Settlement;
            if (origin.MilitiaPartyComponent != null)
            {  
                MobileParty retinue = CreateParty($"bk_retinue_{origin}_{estate}_{MBRandom.RandomInt()}", estate, origin);
                retinue.InitializeMobilePartyAtPosition(
                    origin.Culture.MilitiaPartyTemplate,
                    origin.GatePosition
                 );

                int desiredSize = (int)(estate.MaxManpower.ResultNumber * 0.5f);
                while (retinue.MemberRoster.TotalManCount > desiredSize)
                {
                    var lastTroop = retinue.MemberRoster.GetTroopRoster().Last();
                    retinue.MemberRoster.AddToCounts(lastTroop.Character, -1);
                }

                GiveMounts(ref retinue);
                GiveFood(ref retinue);
                EnterSettlementAction.ApplyForParty(retinue, origin);
                estate.SetParty(retinue);
            }  
        }

        public override void TickHourly()
        {
            var behavior = Behavior;
            if (behavior == AiBehavior.EscortParty)
            {
                MobileParty.SetMoveEscortParty(Escort, MobileParty.NavigationType.All, Escort.IsCurrentlyAtSea);
                if (MobileParty.CurrentSettlement != null) LeaveSettlementAction.ApplyForParty(MobileParty);
            }
            else if (behavior == AiBehavior.GoToSettlement || behavior == AiBehavior.Hold)
            {
                MobileParty.SetMoveGoToSettlement(HomeSettlement, MobileParty.NavigationType.All, HomeSettlement.HasPort);
                if (TaleWorlds.CampaignSystem.Campaign.Current.Models.MapDistanceModel.GetDistance(Party.MobileParty, HomeSettlement, HomeSettlement.HasPort, MobileParty.NavigationType.All, out float distance) <= 2f)
                    EnterSettlementAction.ApplyForParty(Party.MobileParty, HomeSettlement);
            }

            if (MobileParty.CurrentSettlement == null && Behavior != AiBehavior.EscortParty) 
            {
                MobileParty.SetMoveGoToSettlement(HomeSettlement, MobileParty.NavigationType.All, HomeSettlement.HasPort);
            }
        }
    }
}
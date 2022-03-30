using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace Caravanalyzer.Workshop
{
    internal struct WorkshopLogEntry
    {
        public readonly int date;
        public readonly string town;
        public readonly WorkshopType type;
        public readonly string owner;
        public readonly int capital;
        public readonly int daysIdle;
        public readonly int capitalSurplus;
        public readonly int capitalPaidOut;

        public WorkshopLogEntry(int day, TaleWorlds.CampaignSystem.Settlements.Workshops.Workshop workshop, Town town, ClanFinanceModel financeModel)
        {
            this.date = day;
            this.town = town.Name.ToString();
            this.type = workshop.WorkshopType;
            this.owner = workshop.Owner.Name.ToString();
            this.capital = workshop.Capital;
            this.daysIdle = workshop.NotRunnedDays;
            this.capitalSurplus = workshop.ProfitMade;
            this.capitalPaidOut = financeModel.CalculateOwnerIncomeFromWorkshop(workshop);
        }
    }
}

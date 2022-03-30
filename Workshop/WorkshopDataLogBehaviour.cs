using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Caravanalyzer.Workshop
{
    internal class WorkshopDataLogBehaviour : CampaignBehaviorBase
    {
        private readonly List<WorkshopLogEntry> log = new List<WorkshopLogEntry>();

        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickTownEvent.AddNonSerializedListener(this, OnTownDailyTick);
        }

        private void OnTownDailyTick(Town town)
        {
            var financeModel = Campaign.Current.Models.ClanFinanceModel;
            int time = MathF.Floor(Campaign.Current.CampaignStartTime.ElapsedDaysUntilNow);
            foreach (var workshop in town.Workshops)
            {
                WorkshopLogEntry logEntry = new WorkshopLogEntry(time, workshop, town, financeModel);
                log.Add(logEntry);
            }
        }

        public override void SyncData(IDataStore dataStore) { }

        internal void DumpLogToCsv()
        {
            var sb = new StringBuilder();
            var docsDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string csvFullPath = Path.Combine(docsDir, "caravanalyzer-workshop-log.csv");
            try
            {
                using (var writer = new StreamWriter(csvFullPath))
                {
                    writer.WriteLine($"date,town,type,owner,paid_out,capital,days_idle");

                    foreach (var entry in log)
                    {
                        sb.Append(entry.date);
                        sb.Append(",");
                        sb.Append(entry.town);
                        sb.Append(",");
                        sb.Append(entry.type.Name.ToString());
                        sb.Append(",");
                        sb.Append(entry.owner);
                        sb.Append(",");
                        sb.Append(entry.capitalPaidOut);
                        sb.Append(",");
                        sb.Append(entry.capital);
                        sb.Append(",");
                        sb.Append(entry.daysIdle);

                        writer.WriteLine(sb);
                        sb.Clear();
                    }
                }
            }
            catch (IOException e)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Error writing to CSV {csvFullPath}: {e.Message}", TaleWorlds.Library.Colors.Red));
            }

            InformationManager.DisplayMessage(new InformationMessage($"Wrote {log.Count} workshop log entries to {csvFullPath}"));
        }
    }
}

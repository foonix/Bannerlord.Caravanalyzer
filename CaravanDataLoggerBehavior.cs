using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Caravanalyzer
{
    internal class CaravanDataLoggerBehavior : CampaignBehaviorBase
    {
        public readonly List<CaravanCombatLogEntry> log = new List<CaravanCombatLogEntry>();

        public override void RegisterEvents()
        {
            CampaignEvents.MapEventEnded.AddNonSerializedListener(this, OnMapEventEnded);
        }

        private void OnMapEventEnded(MapEvent me)
        {
            if (!me.IsFieldBattle)
            {
                return;
            }

            foreach (var caravanParty in GetMobileCaravansFromSide(me.DefenderSide))
            {
                LogCaravanFight(caravanParty.MobileParty, me.AttackerSide.LeaderParty.MobileParty, me.WinningSide == BattleSideEnum.Defender);

                CaravanCombatLogEntry logEntry;
                logEntry.location = caravanParty.Position2D;
                logEntry.name = caravanParty.Name.ToString();
                logEntry.winningSide = me.WinningSide;

                log.Add(logEntry);
            }
        }

        private IEnumerable<PartyBase> GetMobileCaravansFromSide(MapEventSide side)
        {
            foreach (var mep in side.Parties)
            {
                var mp = mep.Party?.MobileParty;
                if (mp != null && mp.IsCaravan)
                {
                    yield return mep.Party;
                }
            }
        }

        private void LogCaravanFight(MobileParty caravan, MobileParty otherSideLeader, bool caravanWon)
        {
            string success;
            if (otherSideLeader is null)
            {
                success = "drove away";
            }
            else
            {
                success = caravanWon ? "defeated" : "lost to";
            }
            InformationManager.DisplayMessage(new InformationMessage($"Caravan {caravan.Name} {success} {otherSideLeader.Name} at {caravan.GetPosition2D}"));
        }

        public override void SyncData(IDataStore dataStore) { }
    }
}

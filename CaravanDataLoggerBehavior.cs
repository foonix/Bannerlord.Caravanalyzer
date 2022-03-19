using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace Caravanalyzer
{
    internal class CaravanDataLoggerBehavior : CampaignBehaviorBase
    {

        public override void RegisterEvents()
        {
            CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, OnPartyDestroyed);
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
                LogCaravanFight(caravanParty.MobileParty, me.Winner?.LeaderParty?.MobileParty, me.WinningSide == BattleSideEnum.Defender);
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

        private void OnPartyDestroyed(MobileParty mp, PartyBase pb)
        {
            var isCaravan = mp.IsCaravan;
            var pos = mp.Position2D;
            //InformationManager.DisplayMessage(new InformationMessage($"party died is carvan {isCaravan} pos {pos}"));
        }

        public override void SyncData(IDataStore dataStore) { }
    }
}

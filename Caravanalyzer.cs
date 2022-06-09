using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace Caravanalyzer
{
    public class Caravanalyzer : MBSubModuleBase
    {
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (gameStarterObject is CampaignGameStarter gameStarter)
            {
                gameStarter.AddBehavior(new CaravanDataLoggerBehavior());
                gameStarter.AddBehavior(new Workshop.WorkshopDataLogBehaviour());
            }
        }

        public override void OnAfterGameInitializationFinished(Game game, object gameStarterObject)
        {
            Campaign.Current.AddEntityComponent<CaravanCombatVisual>();
            Campaign.Current.AddEntityComponent<Workshop.WorkshopVisual>();
        }
    }
}

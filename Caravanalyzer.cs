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
            }

            Campaign.Current.AddEntityComponent<CaravanCombatVisual>();
        }
    }
}

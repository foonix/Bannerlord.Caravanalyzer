using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace Caravanalyzer
{
    public class Caravanalyzer : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            Module.CurrentModule.AddInitialStateOption(new InitialStateOption("Message",
                new TextObject("Message", null),
                9990,
                () => { InformationManager.DisplayMessage(new InformationMessage("Hello World!")); },
                () => { return (false, null); }
            ));


        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (gameStarterObject is CampaignGameStarter gameStarter)
            {
                gameStarter.AddBehavior(new CaravanDataLoggerBehavior());
                //gameStarter.AddModel(new ModBanditMilitiaKillModel());
            }
        }
    }
}

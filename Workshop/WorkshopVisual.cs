using SandBox.View.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.InputSystem;

namespace Caravanalyzer.Workshop
{
    internal class WorkshopVisual : CampaignEntityVisualComponent
    {
        // for now just using this to hook the dump command.
        public override void OnVisualTick(MapScreen screen, float realDt, float dt)
        {
            var input = screen.SceneLayer.Input;
            if (input.IsKeyPressed(InputKey.W) && input.IsShiftDown())
            {
                var logger = Campaign.Current.GetCampaignBehavior<WorkshopDataLogBehaviour>();
                logger.DumpLogToCsv();
            }
        }
    }
}

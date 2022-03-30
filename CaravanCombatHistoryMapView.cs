using SandBox.View.Map;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace Caravanalyzer
{
    internal class CaravanCombatVisual : CampaignEntityVisualComponent
    {
        private int lastDisplayedLogIndex = 0;

        private bool showCaravanCombatMarkers = false;
        // marker reuse FIFO
        private readonly Queue<CaravanCombatMarker> _markers = new Queue<CaravanCombatMarker>();
        private readonly int maxMarkers = 1000;

        private bool showCaravanDestArrows = false;

        // ARGB color
        private uint destArrowColor = 0xb0ea7712;

        public override void OnVisualTick(MapScreen screen, float realDt, float dt)
        {
            var logger = Campaign.Current.GetCampaignBehavior<CaravanDataLoggerBehavior>();
            var input = screen.SceneLayer.Input;

            if (input.IsKeyDown(InputKey.RightMouseButton) && input.IsKeyDown(InputKey.LeftMouseButton))
            {
                ResetMarkers();
            }

            if (input.IsKeyPressed(InputKey.D) && input.IsShiftDown())
            {
                if (showCaravanCombatMarkers)
                {
                    ResetMarkers();
                }
                showCaravanCombatMarkers = !showCaravanCombatMarkers;
            }

            if (showCaravanCombatMarkers && lastDisplayedLogIndex < logger.log.Count - 1)
            {
                UpdateVisual();
            }

            if (input.IsKeyPressed(InputKey.D) && input.IsAltDown())
            {
                showCaravanDestArrows = !showCaravanDestArrows;
            }

            if (showCaravanDestArrows)
            {
                ShowCaravanDestArrows();
            }
        }

        private void UpdateVisual()
        {
            var logger = Campaign.Current.GetCampaignBehavior<CaravanDataLoggerBehavior>();
            var log = logger.log;

            for (int i = lastDisplayedLogIndex; i < log.Count; i++)
            {
                CaravanCombatLogEntry logEntry = logger.log[i];
                CaravanCombatMarker toUpdate;

                if (_markers.Count >= maxMarkers)
                {
                    toUpdate = _markers.Dequeue();
                }
                else
                {
                    toUpdate = new CaravanCombatMarker();
                }

                toUpdate.ReconfigureFor(logEntry);
                _markers.Enqueue(toUpdate);
                lastDisplayedLogIndex = i;
            }
        }

        private void ResetMarkers()
        {
            foreach (var marker in _markers)
            {
                marker.Dispose();
            }
            _markers.Clear();
            lastDisplayedLogIndex = 0;
        }

        private void ShowCaravanDestArrows()
        {
            foreach (var caravan in MobileParty.AllCaravanParties)
            {
                var destination = caravan.TargetSettlement;

                if (destination is null)
                {
                    continue;
                }

                Vec3 caravanPos = caravan.GetPosition();
                Vec3 destPos = destination.GetPosition();

                MBDebug.RenderDebugDirectionArrow(caravanPos, destPos - caravanPos, destArrowColor, false);
            }
        }
    }
}

using SandBox;
using SandBox.View.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;

namespace Caravanalyzer
{
    internal class CaravanCombatVisual : CampaignEntityVisualComponent
    {
        private int lastDisplayedLogIndex = 0;

        // marker reuse FIFO
        private readonly Queue<CaravanCombatMarker> _markers = new Queue<CaravanCombatMarker>();

        private readonly int maxMarkers = 1000;

        public override void OnVisualTick(MapScreen screen, float realDt, float dt)
        {
            var logger = Campaign.Current.GetCampaignBehavior<CaravanDataLoggerBehavior>();

            if (screen.SceneLayer.Input.IsKeyDown(InputKey.RightMouseButton) && screen.SceneLayer.Input.IsKeyDown(InputKey.LeftMouseButton))
            {
                ResetMarkers();
            }

            if (lastDisplayedLogIndex < logger.log.Count - 1)
            {
                UpdateVisual();
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
    }
}

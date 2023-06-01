using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rewired;
using RoR2;

namespace ScienceKit
{
    public abstract class InputStatistics<T> : BaseRunStatistics<T> where T : IEntry
    {
        private bool _enabled = true;

        protected List<T> Inputs => data;

        protected InputStatistics()
        {
            StartAsyncUpdate();
        }

        private async void StartAsyncUpdate()
        {
            while (_enabled)
            {
                Update();
                await Task.Yield();
            }
        }

        private void Update()
        {
            if (Run.instance == null) return;
            
            if (currentRun != Run.instance)
            {
                Deinitialize();
                Initialize();
            }

            CheckInput();
        }
        
        private void CheckInput()
        {
            if (IsPlayerSendingInput(out var inputPlayer) == false) return;
            LogInputStatistic(inputPlayer);
        }

        protected abstract void LogInputStatistic(Player inputPlayer);

#pragma warning disable Publicizer001
        private bool IsPlayerSendingInput(out Player inputPlayer) => PlayerCharacterMasterController.CanSendBodyInput(
            PlayerCharacterMasterController.instances[0]?.networkUser, out _, out inputPlayer, out _);
#pragma warning restore Publicizer001
        
        public override void Dispose()
        {
            _enabled = false;
            base.Dispose();
        }
    }
}
using TaskManager.Core.Models;

namespace TaskManager.Persistence.Services
{
    public class StateService
    {
        private TaskModel _currentTask;

        public TaskModel CurrentTask
        {
            get => _currentTask;
            set
            {
                _currentTask = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
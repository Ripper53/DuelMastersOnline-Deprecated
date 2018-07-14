using System.Collections.Generic;

namespace DuelMasters {
    public class DuelTaskList {
        public Duelist Owner { get; private set; }
        private List<DuelTask> tasks;
        public int NumberOfTasks => tasks.Count;
        public bool DoingTask => NumberOfTasks > 0;

        public DuelTask this[int index] => tasks[index];

        public DuelTaskList(Duelist owner) {
            Owner = owner;
            tasks = new List<DuelTask>();
        }

        /// <summary>
        /// Returns true if task was completed, false if current turn Duelist is still doing their task.
        /// </summary>
        public bool CompleteTask(int index, object[] args) {
            if (Owner == Owner.Game.CurrentDuelistTurn || !Owner.Game.CurrentDuelistTurn.TaskList.DoingTask) {
                DuelTask task = tasks[index];
                task.CompleteTask(args);
                // Make sure the tasks were not cleared.
                if (NumberOfTasks > 0) {
                    tasks.RemoveAt(index);
                    OnRemovedTask(task, index);
                }
                return true;
            }
            return false;
        }

        public void AddTask(DuelTask task) {
            tasks.Add(task);
            OnAddedTask(task);
        }

        /// <summary>
        /// Return true if task was removed, otherwise false if task was NOT optional and was NOT removed.
        /// </summary>
        public bool RemoveTask(int index) {
            DuelTask task = tasks[index];
            if (task.Optional) {
                tasks.RemoveAt(index);
                OnRemovedTask(task, index);
                return true;
            }
            return false;
        }

        public void ClearOptionalTasks() {
            for (int i = 0; i < tasks.Count; i++) {
                RemoveTask(i);
            }
        }

        public bool AreAllTasksOptional() {
            foreach (DuelTask task in tasks) {
                if (!task.Optional) {
                    return false;
                }
            }
            return true;
        }

        public delegate void TaskAddedEventHandler(DuelTaskList source, DuelTask task);
        public event TaskAddedEventHandler AddedTask;
        protected virtual void OnAddedTask(DuelTask addedTask) {
            AddedTask?.Invoke(this, addedTask);
        }
        public delegate void TaskRemovedEventHandler(DuelTaskList source, DuelTask task, int index);
        public event TaskRemovedEventHandler RemovedTask;
        protected virtual void OnRemovedTask(DuelTask removedTask, int index) {
            RemovedTask?.Invoke(this, removedTask, index);
        }

        public string PrintTasks() {
            string taskStr = "";
            for (int i = 0; i < tasks.Count; i++) {
                taskStr += $"[{i}] {tasks[i].Description}\r\n";
            }
            return taskStr;
        }

    }
}

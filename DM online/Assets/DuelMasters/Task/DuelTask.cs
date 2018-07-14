using System;

namespace DuelMasters {
    public class DuelTask {
        public string Description { get; private set; }
        public bool Optional { get; private set; }
        private Action<object[]> task;
        public Func<object[]> SelectableArgs { get; private set; }

        /// <summary>
        /// Pass in selectableArgs if only certain args can be passed in when completing the task.
        /// </summary>
        public DuelTask(string description, Action<object[]> task, bool optional = true, Func<object[]> selectableArgs = null) {
            Description = description;
            this.task = task;
            Optional = optional;
            SelectableArgs = selectableArgs;
        }

        /// <summary>
        /// Returns true if task was completed, false if args which are not allowed to be passed in, were passed in.
        /// </summary>
        public bool CompleteTask(object[] args) {
            if (ContainArgs(args)) {
                task(args);
                return true;
            }
            return false;
        }

        private bool ContainArgs(object[] args) {
            if (SelectableArgs != null) {
                object[] selectable = SelectableArgs();
                foreach (object arg in args) {
                    if (!SelectableContains(selectable, arg))
                        return false;
                }
            }
            return true;
        }
        private bool SelectableContains(object[] selectable, object item) {
            foreach (object arg in selectable) {
                if (arg is Type) {
                    Type argType = (Type)arg;
                    if (argType == item.GetType())
                        return true;
                } else if (arg == item) {
                    return true;
                }
            }
            return false;
        }

    }
}

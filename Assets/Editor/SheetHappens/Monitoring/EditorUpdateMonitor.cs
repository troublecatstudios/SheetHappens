using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SheetHappens.Monitoring {
    internal class EditorUpdateMonitor {
        public static readonly EditorUpdateMonitor Instance = new EditorUpdateMonitor();

        private readonly List<IMonitorProcess> _processes = new List<IMonitorProcess>();

        private EditorUpdateMonitor() {
            EditorApplication.update += Update;
        }

        public void Monitor(IMonitorProcess process) {
            _processes.Add(process);
        }

        private void Update() {
            int i = 0;
            while (i < _processes.Count) {
                var process = _processes[i];
                var errored = false;
                try {
                    process.Execute();
                } catch (Exception e) {
                    Debug.LogException(e);
                    errored = true;
                }

                if (process.IsComplete || errored) {
                    _processes.RemoveAt(i);
                } else i++;
            }
        }
    }
}

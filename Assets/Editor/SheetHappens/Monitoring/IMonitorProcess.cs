namespace SheetHappens.Monitoring {
    internal interface IMonitorProcess {
        bool IsComplete { get; }
        void Execute();
    }
}

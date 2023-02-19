using System;
using System.Collections.Generic;

namespace SheetHappens {
    [Serializable]
    internal class GoogleSheetResponse<T> {
        public Dictionary<string, GoogleSheet<T>> Sheets { get; set; }
    }

    internal class GoogleSheetVersionResponse {
        public string ScriptVersion { get; set; }
    }

    [Serializable]
    internal class GoogleSheet<T> {
        public string[] Fields { get; set; }
        public T[] Data { get; set; }
    }
}

using System;

namespace SheetHappens {
    internal class GoogleSheetFetchResponse<T> {
        public T Data { get; set; }
        public string Error { get; set; }
        public bool IsError => !string.IsNullOrEmpty(Error);
    }
}

using SheetHappens.Monitoring;
using UnityEngine.Networking;

namespace SheetHappens {
    internal class GoogleSheetApiClient {
        private readonly string _url;
        private UnityWebRequest _webRequest;

        public const string PingAction = "ping";
        public const string ExportAction = "export";

        public GoogleSheetApiClient(string url) {
            _url = url;
        }

        public void CheckVersion(OnFetchCompleteCallback<GoogleSheetVersionResponse> callback) {
            _webRequest = UnityWebRequest.Get($"{_url}?action={PingAction}");
            var op = _webRequest.SendWebRequest();
            var monitor = new GoogleSheetRequestMonitor<GoogleSheetVersionResponse>(_webRequest, callback);
            EditorUpdateMonitor.Instance.Monitor(monitor);
        }

        public void FetchSheetData<T>(OnFetchCompleteCallback<T> callback) {
            _webRequest = UnityWebRequest.Get($"{_url}?action={ExportAction}");
            var op = _webRequest.SendWebRequest();
            var monitor = new GoogleSheetRequestMonitor<T>(_webRequest, callback);
            EditorUpdateMonitor.Instance.Monitor(monitor);
        }
    }
}

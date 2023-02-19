using Newtonsoft.Json;
using SheetHappens.Monitoring;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace SheetHappens {
    internal class GoogleSheetRequestMonitor<T> : GoogleSheetRequestMonitorBase, IMonitorProcess {
        private readonly UnityWebRequest _webRequest;
        private readonly OnFetchCompleteCallback<T> _callback;

        public GoogleSheetRequestMonitor(UnityWebRequest request, OnFetchCompleteCallback<T> callback) {
            _webRequest = request;
            _callback = callback;
            IsComplete = false;
        }

        public void Execute() {
            Debug.Log($"[{nameof(GoogleSheetRequestMonitor<T>)}] DEBUG checking status of WebRequest. status={_webRequest.result}, progress={_webRequest.downloadProgress}, url={_webRequest.url}");
            if (_webRequest != null && _webRequest.isDone) {
                try {
                    var error = _webRequest.error;
                    T result = default(T);
                    if (string.IsNullOrEmpty(error)) {
                        var json = Encoding.UTF8.GetString(_webRequest.downloadHandler.data);
                        Debug.Log($"[{nameof(GoogleSheetRequestMonitor<T>)}] DEBUG received response \n{json}");
                        json = GoogleSheetApiSerializationHelper.ConvertJsonToProperCase(json);
                        result = JsonConvert.DeserializeObject<T>(json);
                    }
                    _callback(new GoogleSheetFetchResponse<T>() {
                        Error = error,
                        Data = result
                    });
                } catch (Exception) {
                    throw;
                } finally {

                    IsComplete = true;
                }
            }
        }
    }
}

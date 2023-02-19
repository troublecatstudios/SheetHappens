using Game;
using Newtonsoft.Json;
using SheetHappens;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace GameEditor {
    public class TranslationsDownloader : IPreprocessBuildWithReport {
        public int callbackOrder => -5;

        public void OnPreprocessBuild(BuildReport report) {
            var client = new GoogleSheetApiClient(Constants.TranslationSheetUrl);
            client.FetchSheetData<GoogleSheetResponse<TranslatedTextItem>>(SynchronizeTranslationsCallback);
        }

        internal static void VersionCallback(GoogleSheetFetchResponse<GoogleSheetVersionResponse> response) {
            if (response.IsError) {
                // do something?
                Debug.LogError($"[{nameof(TranslationsDownloader)}] ERROR {response.Error}");
                return;
            }
            Debug.Log($"[{nameof(TranslationsDownloader)}] INFO DataVariable sheet version: {response.Data.ScriptVersion}");
        }

        internal static void SynchronizeTranslationsCallback(GoogleSheetFetchResponse<GoogleSheetResponse<TranslatedTextItem>> response) {
            if (response.IsError) {
                // do something?
                Debug.LogError($"[{nameof(TranslationsDownloader)}] ERROR {response.Error}");
                return;
            }

            // start synchronizing those variables
            foreach (var sheet in response.Data.Sheets) {
                var assetPath = $"Data/{sheet.Key}.l10n";
                var json = JsonConvert.SerializeObject(sheet.Value.Data);

                Debug.Log($"[{nameof(TranslationsDownloader)}] INFO writing localization file '{assetPath}'");

                File.WriteAllText($"{Application.dataPath}/" + assetPath, json);

                Debug.Log($"[{nameof(TranslationsDownloader)}] INFO importing localization file '{assetPath}'");

                AssetDatabase.ImportAsset("Assets/" + assetPath, ImportAssetOptions.ForceUpdate);
            }
        }
    }
}

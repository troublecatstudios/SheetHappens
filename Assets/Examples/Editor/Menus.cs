using Game;
using SheetHappens;
using UnityEditor;

namespace GameEditor {
    internal class TranslationsGoogleSheetSyncMenu {

        [MenuItem("Tools/SheetHappens - Examples/Translations/Synchronize", priority = 100)]
        public static void Synchronize() {
            var client = new GoogleSheetApiClient(Constants.TranslationSheetUrl);
            client.FetchSheetData<GoogleSheetResponse<TranslatedTextItem>>(TranslationsDownloader.SynchronizeTranslationsCallback);
        }

        [MenuItem("Tools/SheetHappens - Examples/Translations/Check Sheet Version", priority = 100)]
        public static void CheckVersion() {
            var client = new GoogleSheetApiClient(Constants.TranslationSheetUrl);
            client.CheckVersion(TranslationsDownloader.VersionCallback);
        }
    }
}

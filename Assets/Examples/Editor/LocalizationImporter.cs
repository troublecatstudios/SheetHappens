using Game;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace GameEditor {
    [UnityEditor.AssetImporters.ScriptedImporter(1, new[] { ".l10n" })]
    public class LocalizationImporter : UnityEditor.AssetImporters.ScriptedImporter {

        public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx) {
            // process the csv
            var set = ScriptableObject.CreateInstance<LocalizationSet>();
            var keys = new List<string>();
            var values = new List<TranslatedTextItem>();
            var sourceText = File.ReadAllText(ctx.assetPath);

            if (TryParseJSON(sourceText, out var json)) {
                var items = json.ToObject<TranslatedTextItem[]>();
                values.AddRange(items);
                keys.AddRange(items.Select(i => i.Key));
            }

            set.Keys = keys;
            set.Values = values;
            ctx.AddObjectToAsset("LocalizationSet", set);
            ctx.SetMainObject(set);
        }

        private bool TryParseJSON(string fileContents, out JToken json) {
            json = null;
            try {
                json = JToken.Parse(fileContents);
                return true;
            } catch (Exception e) {
                Debug.LogException(e);
                return false;
            }
        }
    }
}

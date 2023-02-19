using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Game {
    [Serializable]
    public class TranslatedTextItem {
        public string Key;
        public List<TranslatedTextValue> Values;

        [JsonExtensionData]
        private IDictionary<string, JToken> _additionalData;

        public string GetValue(string language) {
            foreach (var value in Values) {
                if (value.Language == language) {
                    return value.Value;
                }
            }
            return null;
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context) {
            if (Values == null) Values = new List<TranslatedTextValue>();
            foreach (var data in _additionalData) {
                if (data.Key == "Name") {
                    Key = data.Value.ToString();
                    continue;
                }
                Values.Add(new TranslatedTextValue() { Language = data.Key, Value = data.Value.ToString() });
            }
        }
    }

    [Serializable]
    public class TranslatedTextValue {
        public string Language;
        public string Value;
    }
}

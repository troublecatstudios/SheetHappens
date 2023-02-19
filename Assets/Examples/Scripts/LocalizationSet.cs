using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Game {
        public class LocalizationSet : ScriptableObject {

        public List<string> Keys;
        public List<TranslatedTextItem> Values;
        public List<string> SafeKeys;

        private static Regex Cleanup = new Regex(@"[^0-9a-zA-Z]+");

        public TranslatedTextItem GetItem(string key) {
            int index = SafeKeys.IndexOf(key);
            if (index == -1) index = Keys.IndexOf(key);
            if (index == -1) return null;
            return Values[index];
        }

#if UNITY_EDITOR
        private void OnValidate() {
            SafeKeys.Clear();
            for (var i = 0; i < Keys.Count; i++) {
                var key = Cleanup.Replace(Keys[i], string.Empty).ToLower();
                SafeKeys.Add(key);
            }
        }
#endif
    }
}

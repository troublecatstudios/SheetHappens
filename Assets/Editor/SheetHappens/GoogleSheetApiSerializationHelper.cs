using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace SheetHappens {
    internal static class GoogleSheetApiSerializationHelper {
        public static void ConvertJson(TextReader textReader, TextWriter textWriter, NamingStrategy strategy, Formatting formatting = Formatting.Indented) {
            using (JsonReader reader = new JsonTextReader(textReader))
            using (JsonWriter writer = new JsonTextWriter(textWriter)) {
                writer.Formatting = formatting;
                if (reader.TokenType == JsonToken.None) {
                    reader.Read();
                    ConvertJsonValue(reader, writer, strategy);
                }
            }
        }

        private static void ConvertJsonValue(JsonReader reader, JsonWriter writer, NamingStrategy strategy) {
            if (reader.TokenType == JsonToken.StartObject) {
                writer.WriteStartObject();
                while (reader.Read() && reader.TokenType != JsonToken.EndObject) {
                    string name = strategy.GetPropertyName((string)reader.Value, false);
                    writer.WritePropertyName(name);
                    reader.Read();
                    ConvertJsonValue(reader, writer, strategy);
                }
                writer.WriteEndObject();
            } else if (reader.TokenType == JsonToken.StartArray) {
                writer.WriteStartArray();
                while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                    ConvertJsonValue(reader, writer, strategy);
                }
                writer.WriteEndArray();
            } else if (reader.TokenType == JsonToken.Integer) { // convert integer values to string
                writer.WriteValue(Convert.ToString((long)reader.Value));
            } else if (reader.TokenType == JsonToken.Float) { // convert floating point values to string
                writer.WriteValue(Convert.ToString((double)reader.Value, System.Globalization.CultureInfo.InvariantCulture));
            } else { // string, bool, date, etc.
                writer.WriteValue(reader.Value);
            }
        }

        public static string ConvertJsonToProperCase(string snakeCaseJson) {
            var output = new StringBuilder();
            ConvertJson(new StringReader(snakeCaseJson), new StringWriter(output), new ProperCaseFromSnakeCaseNamingStrategy());
            return output.ToString();
        }

        private class ProperCaseFromSnakeCaseNamingStrategy : NamingStrategy {
            protected override string ResolvePropertyName(string name) {
                StringBuilder sb = new StringBuilder(name.Length);
                for (int i = 0; i < name.Length; i++) {
                    char c = name[i];

                    if (i == 0 || name[i - 1] == '_')
                        c = char.ToUpper(c);

                    if (c != '_')
                        sb.Append(c);
                }
                return sb.ToString();
            }
        }
    }
}

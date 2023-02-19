using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace SheetHappens {
    internal delegate void OnFetchCompleteCallback<T>(GoogleSheetFetchResponse<T> response);
}

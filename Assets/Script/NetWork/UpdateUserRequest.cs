using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateUserRequest : MonoBehaviour
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("level")]
    public int Level { get; set; }

    [JsonProperty("exp")]
    public int Exp { get; set; }

    [JsonProperty("life")]
    public int Life { get; set; }
}

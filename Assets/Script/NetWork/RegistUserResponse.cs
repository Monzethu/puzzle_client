using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class RegistUserResponse
{
    [JsonProperty("user_id")]
    //public int UserID { get; set; }
    public string APIToken { get; internal set; }
}

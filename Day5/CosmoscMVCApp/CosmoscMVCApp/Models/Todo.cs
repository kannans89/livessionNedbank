﻿using Newtonsoft.Json;

namespace CosmoscMVCApp.Models
{
    public class Todo
    {
        [JsonProperty(PropertyName = "id")]
        public String Id { get; set; }

        [JsonProperty(PropertyName = "priority")]
        public string Priority { get; set; } //priority
        public string Title { get; set; }
    }
}

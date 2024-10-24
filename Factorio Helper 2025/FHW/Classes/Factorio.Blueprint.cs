﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Factorio.Blueprint;
//
//    var book = Book.FromJson(jsonString);

namespace Factorio.Blueprint
{
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Book
    {
        [JsonProperty("blueprint_book")]
        public BlueprintBook BlueprintBook { get; set; }
    }

    public partial class BlueprintBook
    {
        [JsonProperty("blueprints")]
        public BlueprintElement[] Blueprints { get; set; }

        [JsonProperty("item")]
        public string Item { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("icons")]
        public Icon[] Icons { get; set; }

        [JsonProperty("active_index")]
        public long ActiveIndex { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }
    }

    public partial class BlueprintElement
    {
        [JsonProperty("blueprint")]
        public BlueprintBlueprint Blueprint { get; set; }

        [JsonProperty("index")]
        public long Index { get; set; }
    }

    public partial class BlueprintBlueprint
    {
        [JsonProperty("icons")]
        public Icon[] Icons { get; set; }

        [JsonProperty("entities")]
        public Entity[] Entities { get; set; }

        [JsonProperty("tiles", NullValueHandling = NullValueHandling.Ignore)]
        public Tile[] Tiles { get; set; }

        [JsonProperty("item")]
        public string Item { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }
    }

    public partial class Entity
    {
        [JsonProperty("entity_number")]
        public long EntityNumber { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("position")]
        public Position Position { get; set; }

        [JsonProperty("direction", NullValueHandling = NullValueHandling.Ignore)]
        public long? Direction { get; set; }

        [JsonProperty("neighbours", NullValueHandling = NullValueHandling.Ignore)]
        public long[] Neighbours { get; set; }

        [JsonProperty("filters", NullValueHandling = NullValueHandling.Ignore)]
        public Filter[] Filters { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("recipe", NullValueHandling = NullValueHandling.Ignore)]
        public string Recipe { get; set; }

        [JsonProperty("output_priority", NullValueHandling = NullValueHandling.Ignore)]
        public string OutputPriority { get; set; }

        [JsonProperty("input_priority", NullValueHandling = NullValueHandling.Ignore)]
        public string InputPriority { get; set; }

        [JsonProperty("connections", NullValueHandling = NullValueHandling.Ignore)]
        public Connections Connections { get; set; }

        [JsonProperty("control_behavior", NullValueHandling = NullValueHandling.Ignore)]
        public ControlBehavior ControlBehavior { get; set; }

        [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
        public string Filter { get; set; }

        [JsonProperty("infinity_settings", NullValueHandling = NullValueHandling.Ignore)]
        public InfinitySettings InfinitySettings { get; set; }

        [JsonProperty("auto_launch", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AutoLaunch { get; set; }

        [JsonProperty("bar", NullValueHandling = NullValueHandling.Ignore)]
        public long? Bar { get; set; }
    }

    public partial class Connections
    {
        [JsonProperty("1")]
        public The1 The1 { get; set; }
    }

    public partial class The1
    {
        [JsonProperty("red")]
        public Red[] Red { get; set; }
    }

    public partial class Red
    {
        [JsonProperty("entity_id")]
        public long EntityId { get; set; }
    }

    public partial class ControlBehavior
    {
        [JsonProperty("circuit_condition")]
        public CircuitCondition CircuitCondition { get; set; }
    }

    public partial class CircuitCondition
    {
        [JsonProperty("first_signal")]
        public Signal FirstSignal { get; set; }

        [JsonProperty("constant")]
        public long Constant { get; set; }

        [JsonProperty("comparator")]
        public string Comparator { get; set; }
    }

    public partial class Signal
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Filter
    {
        [JsonProperty("index")]
        public long Index { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class InfinitySettings
    {
        [JsonProperty("remove_unfiltered_items")]
        public bool RemoveUnfilteredItems { get; set; }
    }

    public partial class Position
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }
    }

    public partial class Icon
    {
        [JsonProperty("signal")]
        public Signal Signal { get; set; }

        [JsonProperty("index")]
        public long Index { get; set; }
    }

    public partial class Tile
    {
        [JsonProperty("position")]
        public Position Position { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Book
    {
        public static Book FromJson(string json) => JsonConvert.DeserializeObject<Book>(json, Factorio.Blueprint.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Book self) => JsonConvert.SerializeObject(self, Factorio.Blueprint.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
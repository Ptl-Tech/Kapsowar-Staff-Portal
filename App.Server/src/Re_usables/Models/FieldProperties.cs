using System.Collections.Generic;

namespace App.Server
{
    public class FieldProperties
    {
        public string Name { get; set; } = "";
        public string? Caption { get; set; }
        public string? DataType { get; set; }
        public string? InputElement { get; set; }
        public string? OptionString { get; set; }
        public string? TableRelationUrl { get; set; }
        public Dictionary<string,string>? ValueStyleIf { get; set; }
        public Dictionary<string, string>? CellStyleIf { get; set; }
        public object? DropdownKeyCaption { get; set; }
    }
    public enum CustomDataTypes
    {
        Text=0,
        Date=1,
        Option=2,
        TableRelation=3,
        Decimal=4,
        Number=5,
    }
}

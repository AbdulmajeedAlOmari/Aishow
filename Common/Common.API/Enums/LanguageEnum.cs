using System.Runtime.Serialization;

namespace Common.API.Enums;

public enum LanguageEnum
{
    [EnumMember(Value = "en")]
    English,
    [EnumMember(Value = "ar")]
    Arabic,
}
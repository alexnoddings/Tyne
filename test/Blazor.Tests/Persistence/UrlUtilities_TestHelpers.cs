using System.Globalization;
using MediatR;

namespace Tyne.Blazor.Persistence;

internal static class UrlUtilities_TestHelpers
{
    public static IEnumerable<object?[]> StringToValue_Data =>
    [
        // Units should skip parsing and always return a Some(Unit)
        ["", Option.Some(Unit.Value)],
        ["blah blah", Option.Some(Unit.Value)],

        // Bool
        ["", Option.None<bool>()],
        ["", Option.None<bool?>()],
        ["  ", Option.None<bool>()],
        ["true", Option.Some(true)],
        ["true", Option.Some<bool?>(true)],
        ["false", Option.Some(false)],
        ["false", Option.Some<bool?>(false)],

        // Int
        ["", Option.None<int>()],
        ["", Option.None<int?>()],
        ["9999999999", Option.None<int>()],
        ["42", Option.Some(42)],
        ["42", Option.Some<int?>(42)],

        // Char
        ["", Option.None<char>()],
        ["", Option.None<char?>()],
        ["TooLong", Option.None<char?>()],
        ["a", Option.Some('a')],
        ["*", Option.Some('*')],

        // Only non-empty-or-whitespace strings should be Some
        ["", Option.None<string>()],
        ["  \t ", Option.None<string>()],
        ["hello", Option.Some("hello")],
        
        // None GUIDs
        ["", Option.None<Guid>()],
        ["", Option.None<Guid?>()],
        ["ThisIsNotValid", Option.None<Guid>()],
        ["RightLengthButInvalid!", Option.None<Guid>()],
        ["FullUnco-mpre-ssed-Leng-thButInvalid", Option.None<Guid>()],
        
        // Compact GUIDs
        ["AAAAAAAAAAAAAAAAAAAAAA", Option.Some(Guid.Empty)],
        ["ULV9Za34j0GZJ13FVSWOeA", Option.Some(Guid.Parse("657db550-f8ad-418f-9927-5dc555258e78"))],
        ["6njQx@%Aj0qp55A6e@pwUQ", Option.Some(Guid.Parse("c7d078ea-80ff-4a8f-a9e7-903a7bfa7051"))],
        ["@@@@@@@@@@@@@@@@@@@@@w", Option.Some(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"))],

        // Regular GUIDs
        ["00000000-0000-0000-0000-000000000000", Option.Some(Guid.Empty)],
        ["657db550-f8ad-418f-9927-5dc555258e78", Option.Some(Guid.Parse("657db550-f8ad-418f-9927-5dc555258e78"))],
        ["c7d078ea-80ff-4a8f-a9e7-903a7bfa7051", Option.Some(Guid.Parse("c7d078ea-80ff-4a8f-a9e7-903a7bfa7051"))],
        ["ffffffff-ffff-ffff-ffff-ffffffffffff", Option.Some(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"))],

        // DateTimes
        ["", Option.None<DateTime>()],
        ["", Option.None<DateTime?>()],
        ["invalid", Option.None<DateTime>()],
        ["20240110080119", Option.Some(DateTime.Parse("2024-01-10T08:01:19.0000000Z", CultureInfo.InvariantCulture))],

        // Enums
        ["", Option.None<SomeEnumType>()],
        ["", Option.None<SomeEnumType?>()],
        [nameof(SomeEnumType.ValueOne), Option.Some(SomeEnumType.ValueOne)],
        [nameof(SomeEnumType.ValueTwo), Option.Some(SomeEnumType.ValueTwo)],
        [nameof(SomeEnumType.ValueOne), Option.Some<SomeEnumType?>(SomeEnumType.ValueOne)],
        [nameof(SomeEnumType.ValueTwo), Option.Some<SomeEnumType?>(SomeEnumType.ValueTwo)],
        [((int)SomeEnumType.ValueOne).ToString(provider: null), Option.Some(SomeEnumType.ValueOne)],
        [((int)SomeEnumType.ValueTwo).ToString(provider: null), Option.Some(SomeEnumType.ValueTwo)],
        [((int)SomeEnumType.ValueOne).ToString(provider: null), Option.Some<SomeEnumType?>(SomeEnumType.ValueOne)],
        [((int)SomeEnumType.ValueTwo).ToString(provider: null), Option.Some<SomeEnumType?>(SomeEnumType.ValueTwo)],

        // JSON fall-back
        ["", Option.None<SerialisableData>()],
        ["{}", Option.Some(new SerialisableData())],
        [@"{""X"":101}", Option.Some(new SerialisableData(101, null, null))],
        [@"{""X"":101,""Y"":true,""Z"":""aBc""}", Option.Some(new SerialisableData(101, true, "aBc"))],

        // Collections
        // Somes are done below as they need to check value equality, which doesn't work with object.Equals(object)
        ["", Option.None<int[]>()],
        ["", Option.None<List<int>>()],
        ["", Option.None<HashSet<int>>()],
    ];

    public static IEnumerable<object?[]> ValueToString_Data =>
    [
        // Null objects should produce a null string
        [null, null],

        // Bool
        [true, "true"],
        [false, "false"],

        // Int
        [42, "42"],

        // Char
        ['x', "x"],

        // Empty/whitespace strings should be removed to de-clutter the URL
        ["", null],
        ["    \t ", null],
        ["abc", "abc"],
        // Strings should be trimmed
        ["\t abc  ", "abc"],

        // Guids are compacted into a special form
        [Guid.Parse("00000000-0000-0000-0000-000000000000"), "AAAAAAAAAAAAAAAAAAAAAA"],
        [Guid.Parse("657db550-f8ad-418f-9927-5dc555258e78"), "ULV9Za34j0GZJ13FVSWOeA"],
        [Guid.Parse("c7d078ea-80ff-4a8f-a9e7-903a7bfa7051"), "6njQx@%Aj0qp55A6e@pwUQ"],
        [Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), "@@@@@@@@@@@@@@@@@@@@@w"],

        // DateTimes should use a compacted string representation
        [DateTime.Parse("2024-01-10T08:01:19.0000000Z", CultureInfo.InvariantCulture), "20240110080119"],

        // Enums should become their names
        [SomeEnumType.ValueOne, nameof(SomeEnumType.ValueOne)],
        [SomeEnumType.ValueTwo, nameof(SomeEnumType.ValueTwo)],

        // JSON fall-back
        [new SerialisableData(101, null, null), @"{""x"":101}"],
        [new SerialisableData(101, true, "aBc"), @"{""x"":101,""y"":true,""z"":""aBc""}"],

        // Array<T> implements ICollection
        [Array.Empty<object>(), null],

        // HashSet<T> doesn't, it only has IEnumerable
        [new HashSet<object>(), null],
    ];
}

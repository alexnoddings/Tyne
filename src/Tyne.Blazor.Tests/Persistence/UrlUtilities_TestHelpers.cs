using System.Globalization;

namespace Tyne.Blazor.Persistence;

// CA1812: Avoid uninstantiated internal classes.
// REASON: Used by TUnit to collect data.
//         Kept as non-static so it is usable as a typeparam.
#pragma warning disable CA1812
internal sealed class UrlUtilities_TestHelpers
{
    private UrlUtilities_TestHelpers() { }

    public static IEnumerable<Func<(string, object)>> GetStringToValueData()
    {
        // Units should skip parsing and always return a Some(Unit)
        yield return () => ("", Option.Some(Unit.Value));
        yield return () => ("blah blah", Option.Some(Unit.Value));

        // Bool
        yield return () => ("", Option.None<bool>());
        yield return () => ("", Option.None<bool?>());
        yield return () => ("  ", Option.None<bool>());
        yield return () => ("true", Option.Some(true));
        yield return () => ("true", Option.Some<bool?>(true));
        yield return () => ("false", Option.Some(false));
        yield return () => ("false", Option.Some<bool?>(false));

        // Int
        yield return () => ("", Option.None<int>());
        yield return () => ("", Option.None<int?>());
        yield return () => ("9999999999", Option.None<int>());
        yield return () => ("42", Option.Some(42));
        yield return () => ("42", Option.Some<int?>(42));

        // Char
        yield return () => ("", Option.None<char>());
        yield return () => ("", Option.None<char?>());
        yield return () => ("TooLong", Option.None<char?>());
        yield return () => ("a", Option.Some('a'));
        yield return () => ("*", Option.Some('*'));

        // Only non-empty-or-whitespace strings should be Some
        yield return () => ("", Option.None<string>());
        yield return () => ("  \t ", Option.None<string>());
        yield return () => ("hello", Option.Some("hello"));

        // None GUIDs
        yield return () => ("", Option.None<Guid>());
        yield return () => ("", Option.None<Guid?>());
        yield return () => ("ThisIsNotValid", Option.None<Guid>());
        yield return () => ("RightLengthButInvalid!", Option.None<Guid>());
        yield return () => ("FullUnco-mpre-ssed-Leng-thButInvalid", Option.None<Guid>());

        // Compact GUIDs
        yield return () => ("AAAAAAAAAAAAAAAAAAAAAA", Option.Some(Guid.Empty));
        yield return () => ("ULV9Za34j0GZJ13FVSWOeA", Option.Some(Guid.Parse("657db550-f8ad-418f-9927-5dc555258e78")));
        yield return () => ("6njQx@%Aj0qp55A6e@pwUQ", Option.Some(Guid.Parse("c7d078ea-80ff-4a8f-a9e7-903a7bfa7051")));
        yield return () => ("@@@@@@@@@@@@@@@@@@@@@w", Option.Some(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff")));

        // Regular GUIDs
        yield return () => ("00000000-0000-0000-0000-000000000000", Option.Some(Guid.Empty));
        yield return () => ("657db550-f8ad-418f-9927-5dc555258e78", Option.Some(Guid.Parse("657db550-f8ad-418f-9927-5dc555258e78")));
        yield return () => ("c7d078ea-80ff-4a8f-a9e7-903a7bfa7051", Option.Some(Guid.Parse("c7d078ea-80ff-4a8f-a9e7-903a7bfa7051")));
        yield return () => ("ffffffff-ffff-ffff-ffff-ffffffffffff", Option.Some(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff")));

        // DateTimes
        yield return () => ("", Option.None<DateTime>());
        yield return () => ("", Option.None<DateTime?>());
        yield return () => ("invalid", Option.None<DateTime>());
        yield return () => ("20240110080119", Option.Some(DateTime.Parse("2024-01-10T08:01:19.0000000Z", CultureInfo.InvariantCulture)));

        // Enums
        yield return () => ("", Option.None<SomeEnumType>());
        yield return () => ("", Option.None<SomeEnumType?>());
        yield return () => (nameof(SomeEnumType.ValueOne), Option.Some(SomeEnumType.ValueOne));
        yield return () => (nameof(SomeEnumType.ValueTwo), Option.Some(SomeEnumType.ValueTwo));
        yield return () => (nameof(SomeEnumType.ValueOne), Option.Some<SomeEnumType?>(SomeEnumType.ValueOne));
        yield return () => (nameof(SomeEnumType.ValueTwo), Option.Some<SomeEnumType?>(SomeEnumType.ValueTwo));
        yield return () => (((int)SomeEnumType.ValueOne).ToString(provider: null), Option.Some(SomeEnumType.ValueOne));
        yield return () => (((int)SomeEnumType.ValueTwo).ToString(provider: null), Option.Some(SomeEnumType.ValueTwo));
        yield return () => (((int)SomeEnumType.ValueOne).ToString(provider: null), Option.Some<SomeEnumType?>(SomeEnumType.ValueOne));
        yield return () => (((int)SomeEnumType.ValueTwo).ToString(provider: null), Option.Some<SomeEnumType?>(SomeEnumType.ValueTwo));

        // JSON fall-back
        yield return () => ("", Option.None<SerialisableData>());
        yield return () => ("{}", Option.Some(new SerialisableData()));
        yield return () => (@"{""X"":101}", Option.Some(new SerialisableData(101, null, null)));
        yield return () => (@"{""X"":101,""Y"":true,""Z"":""aBc""}", Option.Some(new SerialisableData(101, true, "aBc")));

        // Collections
        // Somes are done below as they need to check value equality, which doesn't work with object.Equals(object)
        yield return () => ("", Option.None<int[]>());
        yield return () => ("", Option.None<List<int>>());
        yield return () => ("", Option.None<HashSet<int>>());
    }

    public static IEnumerable<Func<(object?, string?)>> GetValueToStringData()
    {
        // Null objects should produce a null string
        yield return () => (null, null);

        // Bool
        yield return () => (true, "true");
        yield return () => (false, "false");

        // Int
        yield return () => (42, "42");

        // Char
        yield return () => ('x', "x");

        // Empty/whitespace strings should be removed to de-clutter the URL
        yield return () => ("", null);
        yield return () => ("    \t ", null);
        yield return () => ("abc", "abc");
        // Strings should be trimmed
        yield return () => ("\t abc  ", "abc");

        // Guids are compacted into a special form
        yield return () => (Guid.Parse("00000000-0000-0000-0000-000000000000"), "AAAAAAAAAAAAAAAAAAAAAA");
        yield return () => (Guid.Parse("657db550-f8ad-418f-9927-5dc555258e78"), "ULV9Za34j0GZJ13FVSWOeA");
        yield return () => (Guid.Parse("c7d078ea-80ff-4a8f-a9e7-903a7bfa7051"), "6njQx@%Aj0qp55A6e@pwUQ");
        yield return () => (Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), "@@@@@@@@@@@@@@@@@@@@@w");

        // DateTimes should use a compacted string representation
        yield return () => (DateTime.Parse("2024-01-10T08:01:19.0000000Z", CultureInfo.InvariantCulture), "20240110080119");

        // Enums should become their names
        yield return () => (SomeEnumType.ValueOne, nameof(SomeEnumType.ValueOne));
        yield return () => (SomeEnumType.ValueTwo, nameof(SomeEnumType.ValueTwo));

        // JSON fall-back
        yield return () => (new SerialisableData(101, null, null), @"{""x"":101}");
        yield return () => (new SerialisableData(101, true, "aBc"), @"{""x"":101,""y"":true,""z"":""aBc""}");

        // Array<T> implements ICollection
        yield return () => (Array.Empty<object>(), null);

        // HashSet<T> doesn't, it only has IEnumerable
        yield return () => (new HashSet<object>(), null);
    }
}

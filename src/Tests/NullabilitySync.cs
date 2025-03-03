﻿#if NET8_0 && DEBUG
using System.Net.Http;

[TestFixture]
public class NullabilitySync
{
    static string dir;

    static NullabilitySync()
    {
        var solutionDir = SolutionDirectoryFinder.Find();
        dir = Path.Combine(solutionDir, "PolyFill", "Nullability");
    }

    [Test]
    public async Task Run()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        using var client = new HttpClient();
        var infoContext = await client.GetStringAsync("https://raw.githubusercontent.com/dotnet/runtime/main/src/libraries/System.Private.CoreLib/src/System/Reflection/NullabilityInfoContext.cs");

        infoContext = infoContext
            .Replace(".IsGenericMethodParameter", ".IsGenericMethodParameter()")
            .Replace("SR.NullabilityInfoContext_NotSupported", "\"NullabilityInfoContext is not supported\"")
            .Replace(
                "CheckNullabilityAttributes(nullability, setter.GetParametersAsSpan()[^1].GetCustomAttributesData());",
                """
                var parameters = setter.GetParameters();
                                CheckNullabilityAttributes(nullability, parameters[parameters.Length-1].GetCustomAttributesData());
                """)
            .Replace("ReadOnlySpan<ParameterInfo> parameters = metaMethod.GetParametersAsSpan();", "var parameters = metaMethod.GetParameters();")
            .Replace(".GetParametersAsSpan()", ".GetParameters()");

        var lines = infoContext.Split('\r', '\n');
        infoContext = string.Join(Environment.NewLine, lines.Where(_ => !_.Contains("ArgumentNullException.ThrowIfNull")));

        var info = await client.GetStringAsync("https://raw.githubusercontent.com/dotnet/runtime/main/src/libraries/System.Private.CoreLib/src/System/Reflection/NullabilityInfo.cs");

        var prefix = """
                     // <auto-generated />

                     #if !NET6_0_OR_GREATER

                     #nullable enable

                     using System.Linq;
                     using System.Diagnostics.CodeAnalysis;

                     """;

        var suffix = """

                     #endif

                     """;

        infoContext = prefix + infoContext + suffix;
        infoContext = MakeInternal(infoContext);
        OverWrite(infoContext, "NullabilityInfoContext.cs");

        info = prefix + info + suffix;
        info = MakeInternal(info);
        OverWrite(info, "NullabilityInfo.cs");
    }

    static string MakeInternal(string source) =>
        source
            .Replace(
                "public enum",
                """
                    #if PolyPublic
                    public
                    #endif
                    enum
                    """)
            .Replace(
                "public sealed class",
                """
                    [ExcludeFromCodeCoverage]
                    #if PolyPublic
                    public
                    #endif
                    sealed class
                    """);

    static void OverWrite(string content, string file)
    {
        var path = Path.Combine(dir, file);
        File.Delete(path);
        File.WriteAllText(path, content.Trim());
    }
}
#endif
// <auto-generated />

#if !NET5_0_OR_GREATER

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Link = System.ComponentModel.DescriptionAttribute;

namespace System.Runtime.CompilerServices;

/// <summary>
/// Used to indicate to the compiler that a method should be called
/// in its containing module's initializer.
/// </summary>
/// <remarks>
/// When one or more valid methods
/// with this attribute are found in a compilation, the compiler will
/// emit a module initializer which calls each of the attributed methods.
///
/// Certain requirements are imposed on any method targeted with this attribute:
/// - The method must be `static`.
/// - The method must be an ordinary member method, as opposed to a property accessor, constructor, local function, etc.
/// - The method must be parameterless.
/// - The method must return `void`.
/// - The method must not be generic or be contained in a generic type.
/// - The method's effective accessibility must be `internal` or `public`.
///
/// The specification for module initializers in the .NET runtime can be found here:
/// https://github.com/dotnet/runtime/blob/master/docs/design/specs/Ecma-335-Augments.md#module-initializer
/// </remarks>
[Link("https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.moduleinitializerattribute?view=net-7.0")]
[ExcludeFromCodeCoverage]
[DebuggerNonUserCode]
[AttributeUsage(
    validOn: AttributeTargets.Method,
    Inherited = false)]
#if PolyPublic
public
#endif
sealed class ModuleInitializerAttribute :
    Attribute
{
}

#endif
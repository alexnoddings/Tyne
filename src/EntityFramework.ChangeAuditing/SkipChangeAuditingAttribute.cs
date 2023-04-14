namespace Tyne.EntityFramework;

[AttributeUsage(AttributeTargets.Class)]
public sealed class SkipChangeAuditingAttribute : Attribute
{
}

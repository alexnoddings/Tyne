namespace Tyne.EntityFramework;

internal static class DbContextChangeAuditor
{
    public const string IgnoreChangeAuditingAnnotationName = "Tyne:IgnoreChangeAuditing";
    public const string ParentNavigationForAuditingAnnotationName = "Tyne:ParentNavigationForAuditing";
    public const string ChildNavigationForAuditingAnnotationName = "Tyne:ChildNavigationForAuditing";
}

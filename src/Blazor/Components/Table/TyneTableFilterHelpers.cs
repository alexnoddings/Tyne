using System.Linq.Expressions;
using System.Reflection;

namespace Tyne.Blazor;

public static class TyneTableFilterHelpers
{
    public static void UpdatePropertyInfo<TIn, TOut>(Expression<Func<TIn, TOut>>? expression, ref Expression<Func<TIn, TOut>>? cached, ref PropertyInfo? propertyInfo)
    {
        if (expression == cached)
            return;

        cached = expression;

        if (expression is null)
            propertyInfo = null;
        else
            propertyInfo = ExpressionHelper.TryGetAccessedPropertyInfo(expression);
    }
}

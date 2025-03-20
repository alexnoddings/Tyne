namespace Tyne.Blazor;

public delegate Task<Result<Unit, TError>> SaveFormModel<in TModel, TError>(TModel model);

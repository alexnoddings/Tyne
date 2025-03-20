namespace Tyne.Blazor;

public delegate Task<Result<TModel, TError>> MakeFormActive<TModel, TError>(CancellationToken cancellationToken);

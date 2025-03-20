namespace Tyne.Blazor;

public delegate Task<Result<TParams, TError>> InitialiseForm<TParams, TError>();

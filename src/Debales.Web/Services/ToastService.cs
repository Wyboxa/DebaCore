namespace Debales.Web.Services;

public sealed class ToastService
{
    public event Action<ToastMessage>? OnShow;

    public void Show(string text, ToastKind kind = ToastKind.Success) =>
        OnShow?.Invoke(new ToastMessage(Guid.NewGuid(), text, kind));
}

public enum ToastKind { Success, Error, Warning }

public sealed record ToastMessage(Guid Id, string Text, ToastKind Kind);

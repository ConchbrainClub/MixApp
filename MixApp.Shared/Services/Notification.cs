using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;

namespace MixApp.Shared.Services;

class Notification
{
    /// <summary>
    /// When need show toast
    /// </summary>
    public static event Action<ToastParameters<CommunicationToastContent>>? OnToast;

    /// <summary>
    /// Show custome toast in toast container
    /// </summary>
    /// <param name="title">toast title</param>
    /// <param name="content">toast content</param>
    /// <param name="timeout">toast timeout (default is 5s)</param>
    /// <param name="intent">toast intent (default is Success)</param>
    /// <param name="primaryAction">primary action text</param>
    /// <param name="onPrimaryAction">primary action</param>
    /// <param name="secondaryAction">secondary action text</param>
    /// <param name="onSecondaryAction">secondary action</param>
    public static void ShowToast(
        string title, 
        CommunicationToastContent content, 
        int timeout = 5,
        ToastIntent intent = ToastIntent.Success,
        string? primaryAction = null,
        EventCallback<ToastResult>? onPrimaryAction = null,
        string? secondaryAction = null,
        EventCallback<ToastResult>? onSecondaryAction = null)
    {
        OnToast?.Invoke(new ToastParameters<CommunicationToastContent>()
        {
            Intent = intent,
            Title = title,
            Content = content,
            Timeout = timeout,
            PrimaryAction = primaryAction,
            OnPrimaryAction = onPrimaryAction,
            SecondaryAction = secondaryAction,
            OnSecondaryAction = onSecondaryAction
        });
    }
}
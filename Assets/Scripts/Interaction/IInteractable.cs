using System;

public interface IInteractable
{
    public string Message { get; }
    public void Execute(PlayerInteraction sender);
    public void NotifyLookedAt();
    public void NotifyLookedAway();
}

using UnityEngine;

public interface IUIPanel
{
    void OnOpen();
    void OnClose();
    void OnLostFocus();
    void OnGainedFocus();
}

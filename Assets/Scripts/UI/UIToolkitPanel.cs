using UnityEngine;
using UnityEngine.UIElements;

public class UIToolkitPanel : MonoBehaviour, IUIPanel
{
    protected VisualElement Root;

    protected virtual void Awake()
    {
        Root = GetComponent<UIDocument>().rootVisualElement;
    }

    public virtual void OnOpen()
    {
        Root.RemoveFromClassList("hidden");
    }

    public virtual void OnClose()
    {
        Root.AddToClassList("hidden");
    }

    public virtual void OnLostFocus() => Root.AddToClassList("hidden");
    public virtual void OnGainedFocus() => Root.RemoveFromClassList("hidden");
}

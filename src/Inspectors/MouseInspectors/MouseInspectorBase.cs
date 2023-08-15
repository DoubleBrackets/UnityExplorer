namespace UnityExplorer.Inspectors.MouseInspectors
{
    public abstract class MouseInspectorBase
    {
        public static readonly List<GameObject> LastHitObjects = new();

        public abstract void OnBeginMouseInspect();

        public abstract void UpdateMouseInspect(Vector2 mousePos);

        public abstract void OnSelectMouseInspect();

        public abstract void ClearHitData();

        public abstract void OnEndInspect();
    }
}

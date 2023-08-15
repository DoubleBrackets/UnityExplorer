using System.Collections;
using System.Text;
using UnityEngine.EventSystems;
using UnityExplorer.UI;
using UnityExplorer.UI.Panels;

namespace UnityExplorer.Inspectors.MouseInspectors
{
    public class World2DInspector : MouseInspectorBase
    {
        private static Camera MainCamera;

        private static readonly List<GameObject> currentHitObjects = new();


        public override void OnBeginMouseInspect()
        {
            MainCamera = Camera.main;

            if (!MainCamera)
            {
                ExplorerCore.LogWarning("No MainCamera found! Cannot inspect world!");
                return;
            }
        }

        public override void ClearHitData()
        {
            currentHitObjects.Clear();
        }

        public override void OnSelectMouseInspect()
        {
            LastHitObjects.Clear();
            LastHitObjects.AddRange(currentHitObjects);
            RuntimeHelper.StartCoroutine(SetPanelActiveCoro());
        }

        IEnumerator SetPanelActiveCoro()
        {
            yield return null;
            MouseInspectorResultsPanel panel = UIManager.GetPanel<MouseInspectorResultsPanel>(UIManager.Panels.MouseInspectorResults);
            panel.SetActive(true);
            panel.ShowResults();
        }

        public override void UpdateMouseInspect(Vector2 mousePos)
        {
            currentHitObjects.Clear();
            if (!MainCamera)
                MainCamera = Camera.main;
            if (!MainCamera)
            {
                ExplorerCore.LogWarning("No Main Camera was found, unable to inspect world!");
                MouseInspector.Instance.StopInspect();
                return;
            }

            Vector2 pos = MainCamera.ScreenToWorldPoint(mousePos);
            Collider2D[] hitColls = Physics2D.OverlapPointAll(pos);

            var labelBuilder = new StringBuilder();
            
            if (hitColls.Length > 0)
            {
                foreach (Collider2D hit in hitColls)
                {
                    if (hit.gameObject)
                    {
                        currentHitObjects.Add(hit.gameObject);
                        labelBuilder.Append($"\n<color=cyan>{hit.gameObject.name}</color> - {hit.gameObject.transform.GetTransformPath(true)}");
                    }
                }
            }
            else
                MouseInspector.Instance.ClearHitData();

            if (currentHitObjects.Any())
            {
                Debug.Log($"{labelBuilder}");
                MouseInspector.Instance.objNameLabel.text = $"Click to view 2D Coll Objects under mouse: {currentHitObjects.Count} {labelBuilder.ToString()}";
            }
            else
                MouseInspector.Instance.objNameLabel.text = $"No 2D Coll objects under mouse.";
        }

        public override void OnEndInspect()
        {
            // not needed
        }
    }
}

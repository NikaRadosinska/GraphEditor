using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace ExtentionMethods
{
    public class MyExtentions
    {
        /// <summary>
        /// Returns IGraphPart on which one was clicked.
        /// </summary>
        /// <param name="mousePos"></param>
        /// <returns></returns>
        public static IGraphPart GetGraphObjectUnderCursor(Vector2 mousePos)
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, 1 << 9);
            if (hit.collider != null)
            {
                IGraphPart selected = hit.collider.GetComponent<IGraphPart>();
                if (selected == null)
                {
                    selected = hit.transform.GetComponentInParent<IGraphPart>();
                }
                return selected;
            }
            return null;
        }



        ///Returns 'true' if we touched or hovering on Unity UI element.
        public static bool IsPointerOverUIElement()
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults());
        }
        ///Returns 'true' if we touched or hovering on Unity UI element.
        private static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
        {
            for (int index = 0; index < eventSystemRaysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = eventSystemRaysastResults[index];
                if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                    return true;
            }
            return false;
        }
        ///Gets all event systen raycast results of current mouse or touch position.
        private static List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Mouse.current.position.ReadValue();
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            return raysastResults;
        }



    }

}

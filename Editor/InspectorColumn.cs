using System;
using System.Collections.Generic;
using UnityEngine;

namespace KarlBanan.EditorLayout
{
    /// <summary>
    /// Represents a vertical layout element that draws its visible child elements from top to bottom.
    /// </summary>
    /// <remarks>
    /// A column is itself an <see cref="IInspectorElement"/>, which means it can be placed inside
    /// rows or other layout containers. Its width is based on the widest child, and its height is
    /// based on the combined height of all visible children plus spacing and padding.
    /// </remarks>
    public class InspectorColumn : IInspectorElement
    {
        private readonly IInspectorElement[] elements;
        private readonly LayoutSettings settings;
        private readonly Func<bool> canDraw;


        /// <summary>Gets whether the column should currently be drawn.</summary>
        public bool CanDraw => canDraw == null || canDraw();


        /// <summary>Gets whether the column can expand horizontally when additional width is available.</summary>
        /// <remarks>
        /// A column can expand width if any of its visible child elements can expand width.
        /// </remarks>
        public bool ExpandWidth
        {
            get
            {
                for(int i = 0; i < elements.Length; i++)
                {
                    if (elements[i] != null && elements[i].ExpandWidth) return true;
                }
                return false;
            }
        }


        /// <summary>Gets whether the column can expand vertically when additional height is available.</summary>
        /// <remarks>
        /// A column can expand height if any of its visible child elements can expand height.
        /// </remarks>
        public bool ExpandHeight
        {
            get
            {
                for (int i = 0; i < elements.Length; i++)
                {
                    if (elements[i] != null && elements[i].ExpandHeight) return true;
                }
                return false;
            }
        }


        /// <summary>Initializes a new instance of <see cref="InspectorColumn"/> class.</summary>
        /// <param name="settings">The layout settings used when arranging child elements inside the column.</param>
        /// <param name="canDraw">An optional condition that determines whether the column should be drawn.</param>
        /// <param name="elements">The child elements contained inside the column.</param>
        public InspectorColumn(LayoutSettings settings, Func<bool> canDraw = null, params IInspectorElement[] elements)
        {
            this.settings = settings;
            this.canDraw = canDraw;
            this.elements = elements ?? Array.Empty<IInspectorElement>();
        }


        /// <summary>Initializes a new instance of <see cref="InspectorColumn"/> class.</summary>
        /// <param name="settings">The layout settings used when arranging child elements inside the column.</param>
        /// <param name="elements">The child elements contained inside the column.</param>
        public InspectorColumn(LayoutSettings settings, params IInspectorElement[] elements) : this(settings, null, elements)
        {
        }
     

        /// <summary>Gets the minimum height the column can use.</summary>
        /// <returns>The combined heights of all visible children, including spacing and padding.</returns>
        public float GetMinHeight()
        {
            List<IInspectorElement> visible = EditorDraw.GetVisibleElements(elements);
            if (visible.Count == 0) return 0f;

            float totalHeight = settings.PaddingTop + settings.PaddingBottom;
            totalHeight += settings.Spacing * (visible.Count - 1);

            for(int i = 0; i < visible.Count; i++)
            {
                totalHeight += visible[i].GetMinHeight();
            }

            return totalHeight;
        }


        /// <summary>Gets the preferred height the column would like to use.</summary>
        /// <returns>The combined preferred heights of all visible children, including spacing and padding.</returns>
        public float GetPreferredHeight()
        {
            List<IInspectorElement> visible = EditorDraw.GetVisibleElements(elements);
            if (visible.Count == 0) return 0f;

            float totalHeight = settings.PaddingTop + settings.PaddingBottom;
            totalHeight += settings.Spacing * (visible.Count - 1);

            for (int i = 0; i < visible.Count; i++)
            {
                totalHeight += visible[i].GetPreferredHeight();
            }

            return totalHeight;
        }


        /// <summary>Gets the minimum width the column can use.</summary>
        /// <returns>The maximum minimum width of all visible children, including column padding.</returns>
        public float GetMinWidth()
        {
            float maxWidth = 0f;

            for(int i = 0; i < elements.Length; i++)
            {
                if (elements[i] == null || !elements[i].CanDraw) continue;
                maxWidth = Mathf.Max(maxWidth, elements[i].GetMinWidth());
            }
            return maxWidth + settings.PaddingLeft + settings.PaddingRight;
        }


        /// <summary>Gets the minimum preferred width the column would like to use.</summary>
        /// <returns>The maximum preferred width of all visible children, inclding column padding.</returns>
        public float GetPreferredWidth()
        {
            float maxWidth = 0f;

            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i] == null || !elements[i].CanDraw) continue;
                maxWidth = Mathf.Max(maxWidth, elements[i].GetPreferredWidth());
            }
            return maxWidth + settings.PaddingLeft + settings.PaddingRight;
        }
     

        /// <summary>
        /// Draws the column inside the provided area.
        /// </summary>
        /// <param name="rect">The area available for drawing the column.</param>
        /// <remarks>
        /// Child elements are arranged vertically from top to bottom. Remaining height can be distributed
        /// To elements that allow height expansion. If no element expands, the last element can optionally
        /// stretch depending on the layout settings.
        /// </remarks>
        public void Draw(Rect rect)
        {
            if (!CanDraw) return;

            List<IInspectorElement> visible = EditorDraw.GetVisibleElements(elements);
            if (visible.Count == 0) return;

            rect.x += settings.PaddingLeft;
            rect.y += settings.PaddingTop;
            rect.width -= settings.PaddingLeft + settings.PaddingRight;
            rect.height -= settings.PaddingTop + settings.PaddingBottom;

            float totalSpacing = settings.Spacing * (visible.Count - 1);
            float availableHeight = rect.height - totalSpacing;

            float[] heights = new float[visible.Count];
            float totalPreferredHeight = 0f;
            int expandCount = 0;

            // Start by assigning each child its preferred height.
            // While doing so, also count how many children can expand vertically.
            for(int i  = 0; i < visible.Count; i++)
            {
                heights[i] = visible[i].GetPreferredHeight();
                totalPreferredHeight += heights[i];

                if (visible[i].ExpandHeight) expandCount++;
            }

            float leftoverHeight = availableHeight - totalPreferredHeight;

            // If there is extra vertical space available, distribute it
            // to children that allow heigth expansion.
            if(leftoverHeight > 0f)
            {
                if(expandCount > 0)
                {
                    float extraPerExpand = leftoverHeight / expandCount;

                    for(int i = 0; i < visible.Count; i++)
                    {
                        if (visible[i].ExpandHeight) heights[i] += extraPerExpand;
                    }
                }
                else if (settings.StretchLast && visible.Count > 0)
                {
                    heights[^1] += leftoverHeight;
                }
            }

            float y = rect.y;
            for(int i = 0; i<visible.Count; i++)
            {
                IInspectorElement element = visible[i];

                float elementWidth = element.ExpandWidth ? rect.width : Mathf.Min(element.GetPreferredWidth(), rect.width);

                Rect childRect = new(rect.x, y, elementWidth, heights[i]);
                element.Draw(childRect);

                y += heights[i] + settings.Spacing;
            }
        }
    }
}
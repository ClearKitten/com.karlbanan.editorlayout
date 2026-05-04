using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KarlBanan.EditorLayout
{
    /// <summary>
    /// Provides helper methods for drawing inspector layouts using custom <see cref="IInspectorElement"/> instances.
    /// </summary>
    /// <remarks>
    /// This class is responsible for arranging visible inspector elements into rows
    /// using either even width distribution or preferred width distribution.
    /// </remarks>
    public static class EditorDraw
    {
        /// <summary>
        /// Draws a horizontal row where all visible elements receive the same amount of width.
        /// </summary>
        /// <param name="elements">The inspector elements to draw in the row.</param>
        /// <remarks>
        /// This overload uses <see cref="LayoutSettings.Default"/> for spacing and padding.
        /// Hidden or <see langword="null"/> elements are ignored.
        /// </remarks>
        public static void EvenRow(params IInspectorElement[] elements)
        {
            EvenRow(new LayoutSettings(4f, 0f, 0f, 0f, 0f, false), elements);
        }


        /// <summary>
        /// Draws a horizontal row where all visible elements receive the same amount of width.
        /// </summary>
        /// <param name="settings">The layout settings that control spacing, padding and row behaviour.</param>
        /// <param name="elements">The inspector elements to draw in the row.</param>
        /// <remarks>
        /// Hidden or <see langword="null"/> elements are ignored.
        /// Each visible element is placed inside an equal width slot. 
        /// The element itslef decides how much of that slot it actually uses.
        /// </remarks>
        public static void EvenRow(LayoutSettings settings, params IInspectorElement[] elements)
        {
            List<IInspectorElement> visibleElements = GetVisibleElements(elements);
            if (visibleElements.Count == 0) return;

            float height = settings.Height ?? GetRowPreferredHeight(visibleElements);

            Rect rowRect = EditorGUILayout.GetControlRect(false, height);
            rowRect.x += settings.PaddingLeft;
            rowRect.y += settings.PaddingTop;
            rowRect.width -= settings.PaddingLeft + settings.PaddingRight;
            rowRect.height -= settings.PaddingTop + settings.PaddingBottom;

            float totalSpacing = settings.Spacing * (visibleElements.Count - 1);
            float widthPerElement = (rowRect.width - totalSpacing) / visibleElements.Count;

            float x = rowRect.x;
            for(int i = 0; i < visibleElements.Count; i++)
            {
                IInspectorElement element = visibleElements[i];

                // If the element expands height, allow it to use the full row height.
                // Otherwise clamp it to its preferred height and align it on the cross axis.
                float elementHeight = element.ExpandHeight? rowRect.height : Mathf.Min(element.GetPreferredHeight(), rowRect.height);
                float y = GetAlignedY(rowRect, elementHeight, settings.AxisAlignment);

                Rect elementRect = new(x, y, widthPerElement, elementHeight);
                visibleElements[i].Draw(elementRect);

                x += widthPerElement + settings.Spacing;
            }
        }


        /// <summary>
        /// Draws a horizontal row using each visible elements preferred width.
        /// </summary>
        /// <param name="elements">The inspector elements to draw in the row.</param>
        /// <remarks>
        /// This overload uses <see cref="LayoutSettings.Default"/> for spacing and padding.
        /// Hidden or <see langword="null"/> elements are ignored.
        /// </remarks>
        public static void FixedRow(params IInspectorElement[] elements)
        {
            FixedRow(LayoutSettings.Default, elements);
        }


        /// <summary>
        /// Draws a horizontal row using each visible elements preferred width.
        /// </summary>
        /// <param name="settings">The layout settings that control spacing, padding and row behaviour.</param>
        /// <param name="elements">The inspector elements to draw in the row.</param>
        /// <remarks>
        /// Hidden or <see langword="null"/> elements are ignored.
        /// Elements marked with expand width can recive a share of the remaining width.
        /// If no element expands, the last element can optionally stretch depending on the layout settings.
        /// </remarks>
        public static void FixedRow(LayoutSettings settings, params IInspectorElement[] elements)
        {
            List<IInspectorElement> visibleElements = GetVisibleElements(elements);
            if (visibleElements.Count == 0) return;

            float height = settings.Height ?? GetRowPreferredHeight(visibleElements);

            Rect rowRect = EditorGUILayout.GetControlRect(false, height);

            rowRect.x += settings.PaddingLeft;
            rowRect.y += settings.PaddingTop;
            rowRect.width -= settings.PaddingLeft + settings.PaddingRight;
            rowRect.height -= settings.PaddingTop + settings.PaddingBottom;

            float totalSpacing = settings.Spacing * (visibleElements.Count - 1);
            float availableWidth = rowRect.width - totalSpacing;
            
            float[] widths = new float[visibleElements.Count];
            float totalPreferredWidth = 0f;
            int expandCount = 0;

            // Start by assigning every element its preferred width.
            // While doing so, also track how many elements are allowed to expand.
            for(int i = 0; i < visibleElements.Count; i++)
            {
                widths[i] = visibleElements[i].GetPreferredWidth();
                totalPreferredWidth += widths[i];

                if (visibleElements[i].ExpandWidth) expandCount++;
            }

            float leftoverWidth = availableWidth - totalPreferredWidth;

            // If there is remaning width after preferred sizes have been assigne,
            // distribute it between expanding elements.
            if(leftoverWidth > 0f)
            {
                if(expandCount > 0)
                {
                    float extraWidthPerExpandElement = leftoverWidth / expandCount;
                    for(int i = 0; i < visibleElements.Count; i++)
                    {
                        if (visibleElements[i].ExpandWidth) widths[i] += extraWidthPerExpandElement;
                    }
                }
                else if (settings.StretchLast)
                {
                    // If no element expands, optionally let the last element consume the remaining width.
                    widths[^1] += leftoverWidth;
                }
            }

            float x = rowRect.x;
            for(int i = 0; i < visibleElements.Count; i++)
            {
                IInspectorElement element = visibleElements[i];

                // If the element expands height, allow it to use the full row heightr.
                // Otherwise clamp it to its preferred height and align it on the cross axis.
                float elementHeight = element.ExpandHeight ? rowRect.height : Mathf.Min(element.GetPreferredHeight(), rowRect.height);
                float y = GetAlignedY(rowRect, elementHeight, settings.AxisAlignment);

                Rect elementRect = new(x, y, widths[i], elementHeight);
                visibleElements[i].Draw(elementRect);

                x += widths[i] + settings.Spacing;
            }
        }


        private static float GetAlignedY(Rect rowRect, float elementHeight, CrossAxisAlignment alignment)
        {
            return alignment switch
            {
                CrossAxisAlignment.Top => rowRect.y,
                CrossAxisAlignment.Middle => rowRect.y + (rowRect.height - elementHeight) * 0.5f,
                CrossAxisAlignment.Bottom => rowRect.y + (rowRect.height - elementHeight),
                _ => rowRect.y,
            };
        }


        private static float GetRowPreferredHeight(List<IInspectorElement> elements)
        {
            float maxHeight = EditorGUIUtility.singleLineHeight;

            // Use the tallest preferred height so the row can fit all visible elements.
            for(int i = 0; i < elements.Count; i++)
            {
                maxHeight = Mathf.Max(maxHeight, elements[i].GetPreferredHeight());
            }
            return maxHeight;
        }


        /// <summary>
        /// Gets all elements that are not null and can currently be drawn.
        /// </summary>
        /// <param name="elements">The elements to filter.</param>
        /// <returns>A list containing only visible , non-null elements.</returns>
        public static List<IInspectorElement> GetVisibleElements(IInspectorElement[] elements)
        {
            List<IInspectorElement> visibleElements = new();
            if (elements == null) return visibleElements;

            for(int i = 0; i < elements.Length; i++)
            {
                if (elements[i] != null && elements[i].CanDraw) visibleElements.Add(elements[i]);
            }
            return visibleElements;
        }
    }
}
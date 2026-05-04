using UnityEngine;

namespace KarlBanan.EditorLayout
{
    /// <summary>
    /// Represents an element that can be drawn inside a custom inspector layout.
    /// </summary>
    /// <remarks>
    /// Implementations provide, visiblity, sizing, expansion rules, and their own draw behaviour.
    /// </remarks>
    public interface IInspectorElement
    {
        /// <summary>Gets whether the element should currently be drawn.</summary>
        bool CanDraw { get;  }


        /// <summary>Gets whether the element can expand horizontally when additional width is available.</summary>
        bool ExpandWidth { get; }


        /// <summary>Gets whether the element can expand vertically when addtional height is available.</summary>
        bool ExpandHeight { get; }


        /// <summary>Gets the minimum width the element can use.</summary>
        /// <returns>The minimum width in pixels.</returns>
        float GetMinWidth();


        /// <summary>Gets the preferred width the element would like to use.</summary>
        /// <returns>The preferred width in pixels.</returns>
        float GetPreferredWidth();


        /// <summary>Gets the minimum height the element can use.</summary>
        /// <returns>The minimum height in pixels.</returns>
        float GetMinHeight();


        /// <summary>Gets the preferred height the element would like to use.</summary>
        /// <returns>The preferred height in pixels.</returns>
        float GetPreferredHeight();


        /// <summary>Draws the element inside the provided area.</summary>
        /// <param name="rect">The area available for drawing the element.</param>
        void Draw(Rect rect);
    }
}
using System;
using UnityEditor;
using UnityEngine;

namespace KarlBanan.EditorLayout
{
    /// <summary>
    /// Represents an empty inspector element used to reserve or remove space inside a custom layout.
    /// </summary>
    /// <remarks>
    /// This element is useful when manual spacing or alignment is needed inside rows or columns.
    /// </remarks>
    public class InspectorSpace : IInspectorElement
    {
        private readonly Func<bool> canDraw;
        private readonly ElementLayout fieldLayout;
        private readonly bool debugDraw;

        /// <summary>Gets whether the spacer should currently be drawn.</summary>
        public bool CanDraw => canDraw == null || canDraw();


        /// <summary>Gets whether the spacer can expand horizontally when additional width is available.</summary>
        public bool ExpandWidth => fieldLayout.ExpandWidth;


        /// <summary>Gets whether the spacer can expand vertically when additional height is available.</summary>
        public bool ExpandHeight => fieldLayout.ExpandHeight;


        /// <summary>
        /// Initializes a new instance of the <see cref="InspectorSpace"/> class.
        /// </summary>
        /// <param name="fieldLayout">The layout settings that define the reserved size.</param>
        /// <param name="canDisplay">An optional condition that determines whether the spacer should be used.</param>
        /// <param name="debugDraw">Whether the spacer should draw a debug rectangle to make its reserved area visible.</param>
        public InspectorSpace(ElementLayout? fieldLayout = null, Func<bool> canDisplay = null, bool debugDraw = false)
        {
            this.fieldLayout = fieldLayout ?? ElementLayout.Default;
            this.canDraw = canDisplay;
            this.debugDraw = debugDraw;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="InspectorSpace"/> class.
        /// </summary>
        /// <param name="width">The width this spacer will take up.</param>
        /// <param name="canDisplay">An optional condition that determines whether the spacer should be used.</param>
        /// <param name="debugDraw">Whether the spacer should draw a debug rectangle to make its reserved area visible.</param>
        public InspectorSpace(float width, Func<bool> canDisplay = null, bool debugDraw = false)
        {
            fieldLayout = new(width, width, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
            this.canDraw = canDisplay;
            this.debugDraw = debugDraw;
        }


        /// <summary>Gets the minimum height reserved by the spacer.</summary>
        /// <returns>The minimum height in pixels.</returns>
        public float GetMinHeight() => fieldLayout.MinHeight;


        /// <summary>Gets the minimum width reserved by the spacer.</summary>
        /// <returns>The minimum width in pixels.</returns>
        public float GetMinWidth() => fieldLayout.MinWidth;


        /// <summary>Gets the preferred heightreserved by the spacer.</summary>
        /// <returns>The preferred height in pixels.</returns>
        public float GetPreferredHeight() => fieldLayout.PreferredHeight;


        /// <summary>Gets the preferred width reserved by the spacer.</summary>
        /// <returns>The preferred width in pixels.</returns>
        public float GetPreferredWidth() => fieldLayout.PreferredWidth;


        /// <summary>
        /// Draws the spacer inside the provided area.
        /// </summary>
        /// <param name="rect">The area reserved for the spacer.</param>
        /// <remarks>
        /// A spacer normally draws nothing. It simply occupies layout space.
        /// However if <see cref="debugDraw"/> is true, then it draws a red rect where the space is used.
        /// </remarks>
        public void Draw(Rect rect)
        {
            if(!debugDraw || !CanDraw) return;

            EditorGUI.DrawRect(rect, Color.red);
        }
    }
}
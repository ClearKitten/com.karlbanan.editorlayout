using System;
using UnityEditor;
using UnityEngine;

namespace KarlBanan.EditorLayout
{
    /// <summary>
    /// Represents a generic inspector field that stores a value and knows how to draw itself.
    /// </summary>
    /// <typeparam name="T">The value type handled by the field.</typeparam>
    /// <remarks>
    /// This class contains the shared logic for label handling, layout sizing, visibility checks,
    /// and value change detection. Concrete fields only need to implement the actual control drawing.
    /// </remarks>
    public abstract class InspectorField<T> : IInspectorElement
    {
        protected GUIContent content;
        protected Func<bool> canDraw;
        protected T value;
        protected Action<T> setValue;
        protected ElementLayout elementLayout;
        protected LabelSettings labelSettings;


        /// <summary>Gets wheteher the field should currently be drawn.</summary>
        public bool CanDraw => canDraw == null || canDraw();


        /// <summary>Gets whether the field can expand horizontally when additional width is available.</summary>
        public bool ExpandWidth => elementLayout.ExpandWidth;


        /// <summary>Gets whether the field can expand vertically when additional height is available.</summary>
        public bool ExpandHeight => elementLayout.ExpandHeight;


        /// <summary>Gets the minimum height the element can use.</summary>
        /// <returns>The minimum height in pixels.</returns>
        public float GetMinHeight() => elementLayout.MinHeight;


        /// <summary>Gets the minimum width the element can use.</summary>
        /// <returns>The minimum width in pixels, including label and spacing.</returns>
        public float GetMinWidth() => GetTotalWidth(elementLayout.MinWidth);


        /// <summary>Gets the preferred height the element would like to use.</summary>
        /// <returns>The preferred height in pixels, including label and spacing.</returns>
        public float GetPreferredHeight() => elementLayout.PreferredHeight;


        /// <summary>Gets the preferred width the element would like to use.</summary>
        /// <returns>The preferred width in pixels.</returns>
        public float GetPreferredWidth() => GetTotalWidth(elementLayout.PreferredWidth);


        /// <summary>
        /// Initializes a new instance of the <see cref="InspectorField{T}"/> class using GUI content.
        /// </summary>
        /// <param name="content">The content used as the field label.</param>
        /// <param name="value">The starting value of the field.</param>
        /// <param name="setValue">The callback invoked when the value changes.</param>
        /// <param name="canDisplay">An optional condition that determines whether the field should be drawn.</param>
        /// <param name="elementLayout">Optional layout settings for the field.</param>
        /// <param name="labelSettings">Optional label settings for the field.</param>
        protected InspectorField(
            GUIContent content, 
            T value, 
            Action<T> setValue, 
            Func<bool> canDisplay = null, 
            ElementLayout? elementLayout = null, 
            LabelSettings? labelSettings = null)
        {
            this.content = content ?? GUIContent.none;
            this.value = value;
            this.setValue = setValue;
            canDraw = canDisplay;
            this.elementLayout = elementLayout ?? ElementLayout.Default;
            this.labelSettings = labelSettings ?? LabelSettings.LeftDefault;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="InspectorField{T}"/> class using string label.
        /// </summary>
        /// <param name="label">The label text shown for the field.</param>
        /// <param name="value">The starting value of the field.</param>
        /// <param name="setValue">The callback invoked when the value changes.</param>
        /// <param name="canDisplay">An optional condition that determines whether the field should be drawn.</param>
        /// <param name="elementLayout">Optional layout settings for the field.</param>
        /// <param name="labelSettings">Optional label settings for the field.</param>
        protected InspectorField(
            string label, 
            T value, 
            Action<T> 
            setValue, 
            Func<bool> 
            canDisplay = null, 
            ElementLayout? elementLayout = null, 
            LabelSettings? labelSettings = null)
            : this(new GUIContent(label), value, setValue, canDisplay, elementLayout, labelSettings) 
        {
        }

        /// <summary>
        /// Initializes a new instnace of the <see cref="InspectorField{T}"/> class without an initial value callback.
        /// </summary>
        /// <param name="content">The content used as the field label.</param>
        /// <param name="canDisplay">An optional condition that determines whether the field should be drawn.</param>
        /// <param name="elementLayout">Optional layout settings for the field.</param>
        /// <param name="labelSettings">Optional label settings for the field.</param>
        /// <remarks>
        /// Use this constructor for derived fields that manage their values internally or override <see cref="Draw(Rect)"/>.
        /// </remarks>
        protected InspectorField(
            GUIContent content, 
            Func<bool> canDisplay = null, 
            ElementLayout? elementLayout = null, 
            LabelSettings? labelSettings = null)
        {
            this.content = content ?? GUIContent.none;
            canDraw = canDisplay;
            this.elementLayout = elementLayout ?? ElementLayout.Default;
            this.labelSettings = labelSettings ?? LabelSettings.LeftDefault;
        }
       

        /// <summary>
        /// Draws the field inside the provided area.
        /// </summary>
        /// <param name="rect">The area available for drawing the field.</param>
        /// <remarks>
        /// The field draws its label seperately and then draws the control inside 
        /// the remaining area based on its current label and layout settings.
        /// </remarks>
        public virtual void Draw(Rect rect)
        {
            if (!CanDraw) return;

            // Split the incoming rect into a label area and a control area.
            GetRects(rect, content, out Rect labelRect, out Rect fieldRect);

            // Draw the label first so concrete implementations only need
            // to handle drawing the actual control.
            EditorGUI.LabelField(labelRect, content);
            T newValue = DrawField(fieldRect, GUIContent.none, value);

            // Apply the callback if the value changed.
            if(!Equals(newValue, value))
            {
                value = newValue;
                setValue?.Invoke(newValue);
            }
        }
        

        /// <summary>
        /// Gets the width used by the label portion of the field.
        /// </summary>
        /// <param name="content">The label content.</param>
        /// <param name="totalWidth">The total width available to the full field.</param>
        /// <returns>The label width in pixels.</returns>
        protected virtual float GetLabelWidth(GUIContent content, float totalWidth)
        {
            return labelSettings.WidthMode switch
            {
                LabelWidthMode.Auto => EditorStyles.label.CalcSize(content).x,
                LabelWidthMode.Fixed => labelSettings.Width,
                LabelWidthMode.Relative => totalWidth * labelSettings.WidthPercent,
                _ => labelSettings.Width
            };
        }


        /// <summary>
        /// Gets the total width of the field including label and spacing.
        /// </summary>
        /// <param name="fieldWidth">The width of the control portion.</param>
        /// <returns>The total width in pixels.</returns>
        protected virtual float GetTotalWidth(float fieldWidth)
        {
            if(content == null || content == GUIContent.none) return fieldWidth;

            float labelWidth = labelSettings.WidthMode == LabelWidthMode.Relative
                ? labelSettings.Width
                : GetLabelWidth(content, 0f);

            return labelWidth + labelSettings.Spacing + fieldWidth;
        }


        /// <summary>
        /// Calculates the label rect and the control rect for the field.
        /// </summary>
        /// <param name="rect">The available area for the full field.</param>
        /// <param name="content">The label content.</param>
        /// <param name="labelRect">The resulting rect used for drawing the label.</param>
        /// <param name="fieldRect">The resulting rect used for drawing the control.</param>
        /// <remarks>
        /// If the field can expand width, the control portion uses all available remaining width.
        /// Otherwise the control portion is clamped to its preferred width.
        /// </remarks>
        protected virtual void GetRects(Rect rect, GUIContent content, out Rect labelRect, out Rect fieldRect)
        {
            float labelWidth = GetLabelWidth(content, rect.width);
            float spacing = labelSettings.Spacing;

            float availableControlWidth = Mathf.Max(0f, rect.width - labelWidth - spacing);

            float controlWidth = ExpandWidth
                ? availableControlWidth
                : Mathf.Min(elementLayout.PreferredWidth, availableControlWidth);


            if (labelSettings.Side == LabelSide.Left)
            {
                labelRect = new(rect.x, rect.y, labelWidth, rect.height);
                fieldRect = new(rect.x + labelWidth + spacing, rect.y, controlWidth, rect.height);
            }
            else
            {
                fieldRect = new(rect.x, rect.y, controlWidth, rect.height);
                labelRect = new(fieldRect.xMax + spacing, rect.y, labelWidth, rect.height);
            }
        }


        /// <summary>
        /// Draws the concrete control portion of the field.
        /// </summary>
        /// <param name="rect">The area available for the control portion.</param>
        /// <param name="content">The content passed to the control.</param>
        /// <param name="currentValue">The current value of the field.</param>
        /// <returns>The value returned by the drawn control.</returns>
        protected abstract T DrawField(Rect rect, GUIContent content, T currentValue);
    }
}
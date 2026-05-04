namespace KarlBanan.EditorLayout
{
    /// <summary>
    /// Defines how the label portion of an inspector field should be drawn.
    /// </summary>
    /// <remarks>
    /// This struct controls label side, width mode, spacing between label and control.
    /// </remarks>
    public struct LabelSettings
    {
        /// <summary>Gets the side of the field where the label should be drawn.</summary>
        public LabelSide Side { get; private set; }


        /// <summary>Gets the width mode used for the label.</summary>
        public LabelWidthMode WidthMode { get; private set; }


        /// <summary>Gets the spacing between the label and the control.</summary>
        public float Spacing { get; private set; }


        /// <summary>Gets the fixed width used when the label width mode is set to fixed.</summary>
        public float Width { get; private set; }


        /// <summary>Gets the width percent of the label used for <see cref="LabelWidthMode.Relative"/>.</summary>
        public float WidthPercent { get; private set; }
        

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelSettings"/> struct.
        /// </summary>
        /// <param name="side">The side on which the label should be drawn.</param>
        /// <param name="widthMode">The width mode used by the label.</param>
        /// <param name="spacing">The spacing between the label and the control.</param>
        /// <param name="width">The fixed label width used when width mode is <see cref="LabelWidthMode.Fixed"/>.</param>
        /// <param name="widthPercent">The relative label width used when width mode is <see cref="LabelWidthMode.Relative"/>.</param>
        public LabelSettings(LabelSide side, LabelWidthMode widthMode, float spacing, float width = 0f, float widthPercent = 0.4f)
        {
            Side = side;
            WidthMode = widthMode;
            Spacing = spacing;
            Width = width;
            WidthPercent = widthPercent;
        }


        /// <summary>Gets the default left side label settings using automatic width and standard spacing.</summary>
        public static LabelSettings LeftDefault => Auto(LabelSide.Left);


        /// <summary>Gets the default right side label settings using automatic width and standard spacing.</summary>
        public static LabelSettings RightDefault => Auto(LabelSide.Right);


        /// <summary>
        /// Creates label settings that use automatic label width calculation.
        /// </summary>
        /// <param name="side">The side on which the label should be drawn.</param>
        /// <param name="spacing">The spacing between the label and the control.</param>
        /// <returns>
        /// A <see cref="LabelSettings"/> instance configured to calculate label width automatically.
        /// </returns>
        public static LabelSettings Auto(LabelSide side, float spacing = 6f)
            => new(side, LabelWidthMode.Auto, spacing, 0f, 0f);


        /// <summary>
        /// Creates label settings that use a fixed label width.
        /// </summary>
        /// <param name="side">The side on which the label should be drawn.</param>
        /// <param name="width">The fixed width used for the label.</param>
        /// <param name="spacing">The spacing between the label and the control.</param>
        /// <returns>
        /// A <see cref="LabelSettings"/> instance configured to use a fixed label width.
        /// </returns>
        public static LabelSettings Fixed(LabelSide side, float width, float spacing = 6f)
            => new(side, LabelWidthMode.Fixed, spacing, width, 0f);


        /// <summary>
        /// Creates label settings that use a width relative to the available total width.
        /// </summary>
        /// <param name="side">The side on which the label should be drawn.</param>
        /// <param name="widthPercent">The percentage of the available total width used by the label, typically in the range 0 to 1.</param>
        /// <param name="spacing">The spacing between label and the control.</param>
        /// <returns>
        /// A <see cref="LabelSettings"/> instance configured to use a relative label width.
        /// </returns>
        public static LabelSettings Relative(LabelSide side, float widthPercent = 0.45f, float spacing = 6f)
            => new(side, LabelWidthMode.Relative, spacing, 0f, widthPercent);
    }
}
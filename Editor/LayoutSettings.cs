namespace KarlBanan.EditorLayout
{
    /// <summary>
    /// Defines layout settings used when arringing inspector elements inside rows or columns.
    /// </summary>
    /// <remarks>
    /// This structs controls spacing, padding alignment, optional height,
    /// and whether the last element may stretch when no element expand.
    /// </remarks>
    public struct LayoutSettings
    {
        /// <summary>Gets the spacing between elements inside the layout.</summary>
        public float Spacing { get; private set; }
        

        /// <summary>Gets the left padding applied to the layout area.</summary>
        public float PaddingLeft { get; private set; }


        /// <summary>Gets the right padding applied to the layout area.</summary>
        public float PaddingRight { get; private set; }


        /// <summary>Gets the top padding applied to the layout area.</summary>
        public float PaddingTop { get; private set; }


        /// <summary>Gets the bottom padding applied to the layout area.</summary>
        public float PaddingBottom { get; private set; }


        /// <summary>Gets whether the last element should stretch when there is remaining width.</summary>
        public bool StretchLast { get; private set; }


        /// <summary>Gets the axis alignment for a layout.</summary>
        public CrossAxisAlignment AxisAlignment { get; private set;  }


        /// <summary>Optional value to force the height of the layout area.</summary>
        /// <remarks>If this vlaue is <see langword="null"/>, the layout decides its height from the preferred height of its children.</remarks>
        public float? Height { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutSettings"/> struct.
        /// </summary>
        /// <param name="spacing">The spacing between elements.</param>
        /// <param name="paddingLeft">The left padding of the layout.</param>
        /// <param name="paddingRight">The right padding of the layout.</param>
        /// <param name="paddingTop">The top padding of the layout.</param>
        /// <param name="paddingBottom">The bottom padding of the layout.</param>
        /// <param name="stretchLast">Whether the last element may stretch when no element expands.</param>
        /// <param name="axisAlignment">The cross axis alignment used for non expanding elements.</param>
        /// <param name="height">An optional explicit height for the layout.</param>
        public LayoutSettings(float spacing, float paddingLeft, float paddingRight, float paddingTop, float paddingBottom, bool stretchLast, CrossAxisAlignment axisAlignment = CrossAxisAlignment.Top, float? height = null)
        {
            Spacing = spacing;
            PaddingLeft = paddingLeft;
            PaddingRight = paddingRight;
            PaddingTop = paddingTop;
            PaddingBottom = paddingBottom;
            StretchLast = stretchLast;
            AxisAlignment = axisAlignment;
            Height = height;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutSettings"/> struct.
        /// </summary>
        /// <param name="spacing">The spacing between elements.</param>
        /// <param name="axisAlignment">The cross axis alignment used for non expanding elements.</param>
        /// <param name="height">An optional explicit height for the layout.</param>
        public LayoutSettings(float spacing, CrossAxisAlignment axisAlignment, float? height = null) : this(spacing, 0f, 0f, 0f, 0f, false, axisAlignment, height)
        { 
        }


        private static readonly LayoutSettings defaultSettings = new(20f, 0f, 0f, 0f, 0f, false);
        private static readonly LayoutSettings stretchLastSettings = new(20f, 0f, 0f, 0f, 0f, true);
        private static readonly LayoutSettings paddedSettings = new(20f, 6f, 6f, 2f, 2f, false);


        /// <summary>Gets the default layout settings.</summary>
        public static LayoutSettings Default => defaultSettings;


        /// <summary>Gets layout settings where the last element should stretch.</summary>
        public static LayoutSettings StretchLastSettings => stretchLastSettings;


        /// <summary>Gets padded layout settings using default spacing and common padding values.</summary>
        public static LayoutSettings Padded => paddedSettings;
    }
}
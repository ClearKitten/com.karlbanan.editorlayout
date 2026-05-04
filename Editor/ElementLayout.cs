using UnityEditor;

namespace KarlBanan.EditorLayout
{
    /// <summary>
    /// Defines the size and expansion settings for an inspector element.
    /// </summary>
    /// <remarks>
    /// This struct descibes how large an element would like to be, not where it should be placed.
    /// Actual placement is handled by layout helpers such as <see cref="EditorDraw"/> or layout 
    /// elements such as <see cref="InspectorColumn"/>.
    /// </remarks>
    public struct ElementLayout
    {
        /// <summary>Gets the minimium width the element can use.</summary>
        public float MinWidth { get; private set; }


        /// <summary>Gets the preferred width the element would like to use.</summary>
        public float PreferredWidth { get; private set; }


        /// <summary>Gets the minimum height the element can use.</summary>
        public float MinHeight { get; private set; }


        /// <summary>Gets the preferred height the element would like to use.</summary>
        public float PreferredHeight { get; private set; }

        
        /// <summary>Gets whether the element is allowed to expand horizontally when extra width is available.</summary>
        public bool ExpandWidth { get; private set; }


        /// <summary>Gets whether the element is allowed to expand vertically when extra height is available.</summary>
        public bool ExpandHeight { get; private set; }


        /// <summary>
        /// Initalizes a new isntance of the <see cref="ElementLayout"/> struct
        /// </summary>
        /// <param name="minWidth">The minimum control width.</param>
        /// <param name="preferredWidth">The preferred control width.</param>
        /// <param name="minHeight">The minimium control height.</param>
        /// <param name="preferredHeight">The preferred control height.</param>
        /// <param name="expandWidth">Whether the element may expand horizontally.</param>
        /// <param name="expandHeight">Whether the element may expand vertically.</param>
        public ElementLayout(float minWidth, float preferredWidth, float minHeight, float preferredHeight, bool expandWidth = false, bool expandHeight = false)
        {
            MinWidth = minWidth;
            PreferredWidth = preferredWidth;
            MinHeight = minHeight;
            PreferredHeight = preferredHeight;
            ExpandWidth = expandWidth;
            ExpandHeight = expandHeight;
        }


        /// <summary>
        /// Initalizes a new isntance of the <see cref="ElementLayout"/> struct
        /// </summary>
        /// <param name="expandWidth">Whether the element may expand horizontally.</param>
        /// <param name="expandHeight">Whether the element may expand vertically.</param>
        public ElementLayout(bool expandWidth, bool expandHeight) 
            : this(60f, 120f, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight, expandWidth, expandHeight) 
        {
        }

        private static readonly ElementLayout defaultLayout =
            new(60f, 120f, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight, false, false);

        private static readonly ElementLayout expandLayout =
            new(60f, 120f, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight, true, false);

        /// <summary>Gets the default layout for a regular inspector field.</summary>
        public static ElementLayout Default => defaultLayout;


        /// <summary>Gets the default layout controls that should expand horizontally.</summary>
        public static ElementLayout Expand => expandLayout;
    }
}
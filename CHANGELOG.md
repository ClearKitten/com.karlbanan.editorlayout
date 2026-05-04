# Changelog

All notable changes to this package will be documented in this file.

## [1.0.0] - 2026-05-04

### Added

- Initial release of KarlBanan Editor Layout.
- Added the `IInspectorElement` interface for custom drawable inspector elements.
- Added `EditorDraw` row helpers for even width and preferred width horizontal layouts.
- Added `InspectorColumn` for composing inspector elements vertically.
- Added `InspectorSpace` for reserving layout space and optional debug spacing visualization.
- Added the generic `InspectorFielx<T>` base class for creating custom value fields.
- Added `ElementLayout` for minimum/preferred size and expansion settings.
- Added `LayoutSettings` for spacing, padding, fixed height, cross-axis alignment, and stretch-last behaviour.
- Added `LabelSettings`, `LabelSide`, and `LabelWidthMode` for configurable field label layout.
- Added `CrossAxisAlignment` for top, middle and bottom alignment inside rows.
- Added a Core Usage sample for creating a custom inspector element and using that inside a custom editor.
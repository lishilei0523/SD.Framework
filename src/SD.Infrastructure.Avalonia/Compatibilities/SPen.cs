using Avalonia.Media;
using Avalonia.Media.Immutable;
using System.Collections.Generic;
using System.Collections.Specialized;

// ReSharper disable once CheckNamespace
namespace SD.Avalonia.Media
{
    internal class SPen
    {
        internal static bool TryModifyOrCreate(ref IPen pen,
                                     IBrush brush,
                                     double thickness,
                                     IList<double> strokeDashArray = null,
                                     double strokeDaskOffset = default,
                                     PenLineCap lineCap = PenLineCap.Flat,
                                     PenLineJoin lineJoin = PenLineJoin.Miter,
                                     double miterLimit = 10.0)
        {
            IPen previousPen = pen;
            if (brush is null)
            {
                pen = null;
                return previousPen is not null;
            }

            IDashStyle dashStyle = null;
            if (strokeDashArray is { Count: > 0 })
            {
                // strokeDashArray can be IList (instead of AvaloniaList) in future
                // So, if it supports notification - create a mutable DashStyle
                dashStyle = strokeDashArray is INotifyCollectionChanged
                    ? new DashStyle(strokeDashArray, strokeDaskOffset)
                    : new ImmutableDashStyle(strokeDashArray, strokeDaskOffset);
            }

            if (brush is IImmutableBrush immutableBrush && dashStyle is null or ImmutableDashStyle)
            {
                pen = new ImmutablePen(
                    immutableBrush,
                    thickness,
                    (ImmutableDashStyle)dashStyle,
                    lineCap,
                    lineJoin,
                    miterLimit);

                return true;
            }

            Pen mutablePen = previousPen as Pen ?? new Pen();
            mutablePen.Brush = brush;
            mutablePen.Thickness = thickness;
            mutablePen.LineCap = lineCap;
            mutablePen.LineJoin = lineJoin;
            mutablePen.DashStyle = dashStyle;
            mutablePen.MiterLimit = miterLimit;

            pen = mutablePen;
            return !Equals(previousPen, pen);
        }
    }
}

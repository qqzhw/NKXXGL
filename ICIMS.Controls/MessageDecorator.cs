using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ICIMS.Controls
{
    public class MessageDecorator : Decorator
    {
        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(MessageDecorator), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register("BorderThickness", typeof(double), typeof(MessageDecorator), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(Direction), typeof(MessageDecorator), new FrameworkPropertyMetadata(Direction.Right, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(MessageDecorator.OnDirectionPropertyChangedCallback)));
        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(MessageDecorator), new FrameworkPropertyMetadata(new Thickness(25.0, 10.0, 25.0, 10.0), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public MessageDecorator()
        {
            base.HorizontalAlignment = HorizontalAlignment.Right;
            base.Margin = new Thickness(0.0, 5.0, 0.0, 5.0);
            base.Focusable = true;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (this.Child !=null)
            {
                this.Child.Arrange(new Rect(new Point(this.Padding.Left, this.Padding.Top), this.Child.DesiredSize));
            }
            return arrangeSize;
        }

        private Geometry CreateGeometryTailAtLeft()
        {
            CombinedGeometry geometry = new CombinedGeometry();
            Point point = new Point(15.0, 18.0);
            Point point2 = new Point(0.0, 3.0);
            Point point3 = new Point(30.0, 6.0);
            ArcSegment segment = new ArcSegment(point2, new Size(15.0, 20.0), 0.0, false, SweepDirection.Clockwise, true);
            ArcSegment segment2 = new ArcSegment(point3, new Size(18.0, 10.0), 0.0, false, SweepDirection.Counterclockwise, true);
            PathFigure figure = new PathFigure {
                IsClosed = false,
                StartPoint = point
            };
            figure.Segments.Add(segment);
            figure.Segments.Add(segment2);
            PathGeometry geometry2 = new PathGeometry {
                Figures = { figure }
            };
            RectangleGeometry geometry3 = new RectangleGeometry(new Rect(9.0, 0.0, base.ActualWidth - 18.0, base.ActualHeight), 20.0, 20.0);
            geometry.Geometry1 = geometry2;
            geometry.Geometry2 = geometry3;
            geometry.GeometryCombineMode = GeometryCombineMode.Union;
            return geometry;
        }

        private Geometry CreateGeometryTailAtRight()
        {
            CombinedGeometry geometry = new CombinedGeometry();
            Point point = new Point(base.ActualWidth - 15.0, 18.0);
            Point point2 = new Point(base.ActualWidth, 0.0);
            Point point3 = new Point(base.ActualWidth - 30.0, 5.0);
            ArcSegment segment = new ArcSegment(point2, new Size(15.0, 20.0), 0.0, false, SweepDirection.Counterclockwise, true);
            ArcSegment segment2 = new ArcSegment(point3, new Size(18.0, 10.0), 0.0, false, SweepDirection.Clockwise, true);
            PathFigure figure = new PathFigure {
                IsClosed = false,
                StartPoint = point
            };
            figure.Segments.Add(segment);
            figure.Segments.Add(segment2);
            PathGeometry geometry2 = new PathGeometry {
                Figures = { figure }
            };
            RectangleGeometry geometry3 = new RectangleGeometry(new Rect(9.0, 0.0, base.ActualWidth - 18.0, base.ActualHeight), 20.0, 20.0);
            geometry.Geometry1 = geometry2;
            geometry.Geometry2 = geometry3;
            geometry.GeometryCombineMode = GeometryCombineMode.Union;
            return geometry;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size();
            if (this.Child != null)
            {
                this.Child.Measure(constraint);
                size.Width = this.Child.DesiredSize.Width;
                size.Height = (this.Child.DesiredSize.Height + this.Padding.Top) + this.Padding.Bottom;
            }
            return size;
        }

        private void OnDirectionPropertyChangedCallback()
        {
            base.HorizontalAlignment = this.Direction.ToString().ToEnumFromName<HorizontalAlignment>();
        }

        public static void OnDirectionPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MessageDecorator) d).OnDirectionPropertyChangedCallback();
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (this.Child != null)
            {
                Geometry geometry = null;
                Brush brush = null;
                Pen pen = new Pen {
                    Brush = this.BorderBrush,
                    Thickness = this.BorderThickness
                };
                if (this.Direction ==Direction.Right)
                {
                    geometry = this.CreateGeometryTailAtRight();
                }
                else
                {
                    geometry = this.CreateGeometryTailAtLeft();
                }
                dc.DrawGeometry(brush, pen, geometry);
            }
        }

        public Brush BorderBrush
        {
            get
            {
                return (Brush) base.GetValue(BorderBrushProperty);
            }
            set
            {
                base.SetValue(BorderBrushProperty, value);
            }
        }

        public double BorderThickness
        {
            get
            {
                return (double) base.GetValue(BorderThicknessProperty);
            }
            set
            {
                base.SetValue(BorderThicknessProperty, value);
            }
        }

        public Direction Direction
        {
            get
            {
                return (Direction) base.GetValue(DirectionProperty);
            }
            set
            {
                base.SetValue(DirectionProperty, value);
            }
        }

        public Thickness Padding
        {
            get
            {
                return (Thickness) base.GetValue(PaddingProperty);
            }
            set
            {
                base.SetValue(PaddingProperty, value);
            }
        }
    }
}


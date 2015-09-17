namespace EliMarshal.ClassLibrary.AttributePropertyHelper
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    /// <summary>
    /// Extension to set properties on WPF controls based on attributes on their binding properties
    /// </summary>
    public static class AttributePropertyExtension
    {
        /// <summary>
        /// DependencyProperty set on every control to affect
        /// </summary>
        public static readonly DependencyProperty AttributeProperty = DependencyProperty.RegisterAttached(
             "AttributeProperty", typeof(bool), typeof(AttributePropertyExtension), new UIPropertyMetadata(false, OnAttributePropertyChanged));

        /// <summary>
        /// Gets the AttributeProperty DependencyProperty
        /// </summary>
        /// <param name="obj">The framework element object.</param>
        /// <returns>true if the specified value is valid; otherwise, false.</returns>
        public static bool GetAttributeProperty(FrameworkElement obj)
        {
            return (bool)obj.GetValue(AttributeProperty);
        }

        /// <summary>
        /// Sets the AttributeProperty DependencyProperty
        /// </summary>
        /// <param name="obj">The framework element object.</param>
        /// <param name="value">The value to set AttributeProperty to.</param>
        public static void SetAttributeProperty(FrameworkElement obj, bool value)
        {
            obj.SetValue(AttributeProperty, value);
        }

        /// <summary>
        /// Assigns the AttributeProperty to the control when AttributeProperty changes.
        /// </summary>
        /// <param name="d">The DependencyObject.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs.</param>
        private static void OnAttributePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uie = (UIElement)d;
            if ((bool)e.NewValue && uie.Dispatcher != null)
            {
                if (uie is TextBox)
                {
                    AssignAttributePropertyToControl<Control>(d, e);
                }

                uie.SetValue(AttributeProperty, false);
            }
        }

        // Use this space for a different dependency property added to a different type of control in xaml

        /// <summary>
        /// Adds the AttributePropertyBehavior to the AttributeProperty
        /// </summary>
        /// <typeparam name="T">The first generic type parameter.</typeparam>
        /// <param name="d">The DependencyProperty AttributeProperty.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs for AttributeProperty.</param>
        private static void AssignAttributePropertyToControl<T>(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behaviors = Interaction.GetBehaviors(d);

            if ((bool)e.NewValue)
            {
                if (!behaviors.OfType<T>().Any())
                {
                    behaviors.Add(new AttributePropertyBehavior());
                }
            }
            else
            {
                foreach (var item in behaviors.ToArray())
                {
                    if (item is T)
                    {
                        behaviors.Remove(item);
                    }
                }
            }
        }

        /// <summary>
        /// The behavior to set the attribute property on the control.
        /// </summary>
        private class AttributePropertyBehavior : Behavior<DependencyObject>
        {
            /// <summary>
            /// When attached, sets the binding attribute property on the control.
            /// </summary>
            protected override void OnAttached()
            {
                if (AssociatedObject is Control)
                {
                    (AssociatedObject as Control).Loaded += (sender, args)
                        => AttributePropertySetter.SetAttributeProperty(AssociatedObject as Control);
                }

                // Use this space to set AttributeProperty to different types than Control, i.e. FrameworkContentElement
                base.OnAttached();
            }
        }
    }
}

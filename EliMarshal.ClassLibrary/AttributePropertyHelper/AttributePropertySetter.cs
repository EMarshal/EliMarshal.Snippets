namespace EliMarshal.ClassLibrary.AttributePropertyHelper
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// Sets the binding attribute property value on the editor control.
    /// </summary>
    public static class AttributePropertySetter
    {
        /// <summary>
        /// Sets the binding attribute property value on the editor control.
        /// </summary>
        /// <param name="editor">The editor control.</param>
        public static void SetAttributeProperty(Control editor)
        {
            object context = editor.DataContext;
            BindingExpression binding = GetBindingExpression(editor);
            if (context == null || binding == null)
            {
                return;
            }

            if (binding.ResolvedSourcePropertyName != null && binding.ResolvedSource != null)
            {
                PropertyInfo prop = binding.ResolvedSource.GetType().GetProperty(binding.ResolvedSourcePropertyName);
                AssignAttributePropertyFromProperty(editor, prop);
                return;
            }

            // Can't effectively test this as is, as .GetProperty() can't be intercepted by FakeItEasy
            PropertyInfo itemProp = context.GetType().GetProperty("Item");
            if (itemProp != null && itemProp.GetSetMethod() != null)
            {
                var item = itemProp.GetValue(context);
                if (item != null)
                {
                    PropertyInfo prop = item.GetType().GetProperty(binding.ParentBinding.Path.Path);
                    AssignAttributePropertyFromProperty<Control>(editor, prop);
                }
            }
        }

        // Use this space for alternate SetAttributeProperty methods with different paramaters, i.e. FrameworkContentElement rather than Control

        /// <summary>
        /// Gets the binding expression for the editor.
        /// </summary>
        /// <param name="editor">The editor control.</param>
        /// <returns>BindingExpression for the TextProperty if the editor is a TextBox, ArgumentException otherwise.</returns>
        private static BindingExpression GetBindingExpression(Control editor)
        {
            if (editor is TextBox)
            {
                return editor.GetBindingExpression(TextBox.TextProperty);
            }
            else
            {
                throw new ArgumentException("Not a valid control type.");
            }

            // Use the above flow to get binding expressions for editors and properties other than TextBox.TextProperty
        }

        /// <summary>
        /// Sets the property on the editor based on the binding property attribute.
        /// </summary>
        /// <typeparam name="T">The first generic type parameter.</typeparam>
        /// <param name="editor">The editor control.</param>
        /// <param name="prop">The bound property.</param>
        private static void AssignAttributePropertyFromProperty<T>(T editor, PropertyInfo prop)
        {
            if (prop != null)
            {
                var stringLengthAtt = Attribute.GetCustomAttributes(prop, typeof(StringLengthAttribute), true).FirstOrDefault() as StringLengthAttribute;
                if (stringLengthAtt != null)
                {
                    if (editor is TextBox)
                    {
                        (editor as TextBox).MaxLength = stringLengthAtt.MaximumLength;
                    }

                    // Use this space set property to different editor types and/or properties other than TextBox.MaxLength
                }

                // Use this space to get different custom attributes other than StringLengthAttribute
            }
        }
    }
}

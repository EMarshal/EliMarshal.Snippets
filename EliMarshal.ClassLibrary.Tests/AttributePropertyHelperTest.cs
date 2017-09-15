namespace EliMarshal.ClassLibrary.Tests
{
    using AttributePropertyHelper;
    using NSubstitute;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Xunit;

    public class AttributePropertyHelperTest
    {
        private TextBox editor;

        public AttributePropertyHelperTest()
        {
            editor = new TextBox();
        }

        [WpfFact]
        public void TestSetMaxLengthWithNullBinding()
        {
            editor.DataContext = Substitute.For<object>();
            AttributePropertySetter.SetAttributeProperty(editor);
            Assert.Equal(0, editor.MaxLength);
        }

        [WpfFact]
        public void TestSetMaxLengthWithNullDataContext()
        {
            StringLengthTestClass testObject = new StringLengthTestClass();
            Binding myBinding = new Binding("StringLengthProperty");
            myBinding.Source = testObject;

            editor.SetBinding(TextBox.TextProperty, myBinding);
            AttributePropertySetter.SetAttributeProperty(editor);
            Assert.Equal(0, editor.MaxLength);
        }

        [WpfFact]
        public void TestSetMaxLengthWithInvalidControlType()
        {
            var invalidEditor = new RichTextBox();
            StringLengthTestClass testObject = new StringLengthTestClass();
            Binding myBinding = new Binding("StringLengthProperty");
            myBinding.Source = testObject;

            invalidEditor.SetBinding(TextBox.TextProperty, myBinding);
            Assert.Throws<ArgumentException>(() => AttributePropertySetter.SetAttributeProperty(invalidEditor));
        }

        [WpfFact]
        public void TestSetMaxLengthBindingHasResolvedSourceWithTextBinding()
        {
            editor.DataContext = Substitute.For<object>();
            StringLengthTestClass testObject = new StringLengthTestClass();
            Binding myBinding = new Binding("StringLengthProperty");
            myBinding.Source = testObject;

            editor.SetBinding(TextBox.TextProperty, myBinding);
            AttributePropertySetter.SetAttributeProperty(editor);
            Assert.Equal(50, editor.MaxLength);
        }

        [WpfFact]
        public void TestSetMaxLengthBindingDoesNotHaveResolvedSourceAndContextItemIsNull()
        {
            editor.DataContext = Substitute.For<object>();
            Binding myBinding = new Binding("StringLengthProperty");

            editor.SetBinding(TextBox.TextProperty, myBinding);
            AttributePropertySetter.SetAttributeProperty(editor);
            Assert.Equal(0, editor.MaxLength);
        }

        [WpfFact]
        public void TestSetMaxLengthBindingWhenAttributeIsInherited()
        {
            editor.DataContext = Substitute.For<object>();
            StringLengthTestClass myMockObject = new ChildStringLengthTestClass();
            Binding myBinding = new Binding("StringLengthProperty");
            myBinding.Source = myMockObject;

            editor.SetBinding(TextBox.TextProperty, myBinding);
            AttributePropertySetter.SetAttributeProperty(editor);
            Assert.Equal(50, editor.MaxLength);
        }

        private class StringLengthTestClass
        {
            [StringLength(50)]
            public virtual string StringLengthProperty { get; set; }
        }

        private class ChildStringLengthTestClass : StringLengthTestClass
        {
            public override string StringLengthProperty { get; set; }
        }
    }
}

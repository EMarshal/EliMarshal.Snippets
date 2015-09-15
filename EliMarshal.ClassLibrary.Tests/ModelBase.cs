namespace EliMarshal.ClassLibrary.Tests
{
    using System.ComponentModel.DataAnnotations;

    internal abstract class ModelBase<T> where T : ValidationAttribute
    {
        public T GetClassAttribute()
        {
            return (T)this.GetType().GetCustomAttributes(typeof(T), false)[0];
        }

        public T GetPropertyAttribute(string property)
        {
            return (T)this.GetType().GetProperty(property).GetCustomAttributes(typeof(T), false)[0];
        }

        public bool IsValid()
        {
            var attribute = this.GetClassAttribute();
            return attribute.IsValid(this);
        }

        public bool IsValid(string property)
        {
            var attribute = this.GetPropertyAttribute(property);
            return attribute.IsValid(this.GetType().GetProperty(property).GetValue(this, null));
        }
    }
}

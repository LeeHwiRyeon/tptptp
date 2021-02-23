using System;

namespace SimpleProfiler {
    //[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
    //public class CallerInfoAttribute : Attribute {
    //    public int SomeValue { get; private set; }
    //    //public CallerInfoAttribute(RuntimeParameterInfo runtimeParameterInfo)
    //    //{
            
    //    //}

    //}

    [AttributeUsage(AttributeTargets.All)]
    public class CallerInfoAttribute : Attribute {
        // Private fields.
        private string name;
        private int handle;
        private bool reviewed;

        // This constructor defines two required parameters: name and level.

        public CallerInfoAttribute()
        {
        }

        // Define Name property.
        // This is a read-only attribute.

        public virtual string Name {
            get { return name; }
        }

        // Define Level property.
        // This is a read-only attribute.

        public virtual int Handle {
            get { return handle; }
        }

        // Define Reviewed property.
        // This is a read/write attribute.

        public virtual bool Reviewed {
            get { return reviewed; }
            set { reviewed = value; }
        }
    }
}
using System;

namespace SimpleProfiler {
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
    public class CallerInfoAttribute : Attribute {
        public int SomeValue { get; private set; }
        //public CallerInfoAttribute(RuntimeParameterInfo runtimeParameterInfo)
        //{
            
        //}

    }
}
using System;

namespace SR.Framework.Autofac
{
    public class InjectableServiceAttribute : Attribute
    {
        public bool IsSingletone { get; set; }
    }
}

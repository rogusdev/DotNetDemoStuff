using System;
using Nancy;

namespace DemoNancy
{
    public class BaseModule : NancyModule
    {
        public BaseModule() : base("/")
        {
            Get("/", args => "NancyFX Hello @ " + DateTime.UtcNow);
        }
    }
}

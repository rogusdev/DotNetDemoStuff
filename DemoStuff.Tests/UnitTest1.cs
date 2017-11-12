using System;
using Xunit;

namespace DemoStuff.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var now = DateTime.UtcNow;
            var id = Guid.NewGuid();

            var newThing = new Thing()
            {
                Id = id,
                Name = $"Hello world {now}",
                Enabled = true,
                CreatedAt = now.AddMinutes(-10),
                UpdatedAt = now.AddMinutes(10),
            };

            // trivial assertions to demo asserting with xunit using referenced library
            Assert.Equal(id, newThing.Id);
            Assert.Equal($"Hello world {now}", newThing.Name);
            Assert.Equal(TimeSpan.FromMinutes(20),
                newThing.UpdatedAt - newThing.CreatedAt);
        }
    }
}

using System;

using FluentAssertions;

using Xunit;

namespace Wasmtime.Tests
{
    public class OutOfBoundsMemoryAccessFixture : ModuleFixture
    {
        protected override string ModuleFileName => "OutOfBoundsMemoryAccess.wat";
    }

    public class OutOfBoundsMemoryAccessTests : IClassFixture<OutOfBoundsMemoryAccessFixture>
    {
        public OutOfBoundsMemoryAccessTests(OutOfBoundsMemoryAccessFixture fixture)
        {

            Fixture = fixture;

            Fixture.Host.ClearDefinitions();
        }

        private OutOfBoundsMemoryAccessFixture Fixture { get; set; }

        [Fact]
        public void ItDoesNotThrowsOnOutOfBoundsMemoryAccess()
        {
            var mem = Fixture.Host.DefineMemory("", "mem", minimum: 2, maximum: 2);

            using var instance = Fixture.Host.Instantiate(Fixture.Module);

            mem.ReadByte(65535).Should().Be(1);
            mem.ReadByte(65536).Should().Be(2);
        }

        [Fact]
        public void ItThrowsOnOutOfBoundsMemoryAccess()
        {
            var mem = Fixture.Host.DefineMemory("", "mem", minimum: 1, maximum: 1);

            Action action = () => { using var instance = Fixture.Host.Instantiate(Fixture.Module); };

            action
                .Should()
                .Throw<TrapException>()
                .WithMessage("wasm trap: out of bounds memory access, source location: @-");
        }
    }
}
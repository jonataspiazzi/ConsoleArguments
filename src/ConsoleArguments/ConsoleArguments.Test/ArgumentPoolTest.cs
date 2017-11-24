using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ConsoleArguments.Test
{
    [TestClass]
    public class ArgumentPoolTest
    {
        [TestMethod]
        public void ShouldGenerateHelpText()
        {
            var argPool = new ArgumentPool();

            argPool
                .BuildArgument("-i")
                .HasAlias("--init")
                .HasBindArgs("filename")
                .HasDescription("Init something.")
                .IsRequired();

            argPool
                .BuildArgument("-f")
                .HasAlias("--force")
                .HasDescription("Force some stuff.");

            var text = argPool.GetHelperText("cmdname");

            var expectation =
                "cmdname -i filename [-f]" + Environment.NewLine +
                "  -i,--init filename  Init something." + Environment.NewLine +
                "  [-f,--force]        Force some stuff."+ Environment.NewLine;

            Assert.AreEqual(expectation, text);
        }

        private ArgumentPool CreateSimpleArgumentPool(Action<ArgumentFoundEventArgs> p1OnFound, Action<ArgumentFoundEventArgs> p2OnFound)
        {
            var argPool = new ArgumentPool();

            argPool
                .BuildArgument("-p1")
                .HasAlias("--param1")
                .HasBindArgs("num1", "num2")
                .OnArgumentFound((sender, e) => p1OnFound(e));

            argPool
                .BuildArgument("-p2")
                .HasAlias("--param2")
                .HasBindArgs("num")
                .OnArgumentFound((sender, e) => p2OnFound(e));

            return argPool;
        }

        [TestMethod]
        public void ShoudProcessWithName()
        {
            var a = 0;
            var b = 0;
            var c = 0;

            var pool = CreateSimpleArgumentPool(
                e =>
                {
                    a = int.Parse(e.BindArguments[0]);
                    b = int.Parse(e.BindArguments[1]);
                    e.IsValid = true;
                },
                e =>
                {
                    c = int.Parse(e.BindArguments[0]);
                    e.IsValid = true;
                });

            pool.ProcessArguments("-p1 10 20 -p2 30".Split(new[] { ' ' }));

            Assert.AreEqual(10, a);
            Assert.AreEqual(20, b);
            Assert.AreEqual(30, c);
        }

        [TestMethod]
        public void ShoudProcessWithAlias()
        {
            var a = 0;
            var b = 0;
            var c = 0;

            var pool = CreateSimpleArgumentPool(
                e =>
                {
                    a = int.Parse(e.BindArguments[0]);
                    b = int.Parse(e.BindArguments[1]);
                    e.IsValid = true;
                },
                e =>
                {
                    c = int.Parse(e.BindArguments[0]);
                    e.IsValid = true;
                });

            pool.ProcessArguments("--param1 10 20 -p2 30".Split(new[] { ' ' }));

            Assert.AreEqual(10, a);
            Assert.AreEqual(20, b);
            Assert.AreEqual(30, c);
        }
    }
}

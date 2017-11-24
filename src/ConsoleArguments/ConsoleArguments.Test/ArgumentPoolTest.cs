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

        [TestMethod]
        public void ShoudProcess()
        {
            var a = 0;
            var b = 0;
            var c = 0;

            var argPool = new ArgumentPool();

            argPool
                .BuildArgument("-p1")
                .HasBindArgs("num1", "num2")
                .OnArgumentFound((sender, e) =>
                {
                    a = int.Parse(e.BindArguments[0]);
                    b = int.Parse(e.BindArguments[1]);
                    e.IsValid = true;
                });

            argPool
                .BuildArgument("-p2")
                .HasBindArgs("num")
                .OnArgumentFound((sender, e) =>
                {
                    c = int.Parse(e.BindArguments[0]);
                    e.IsValid = true;
                });

            argPool.ProcessArguments("-p1 10 20 -p2 30".Split(new[] { ' ' }));

            Assert.AreEqual(10, a);
            Assert.AreEqual(20, b);
        }
    }
}

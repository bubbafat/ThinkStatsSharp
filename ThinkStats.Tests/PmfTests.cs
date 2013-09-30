using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThinkStats.Tests
{
    [TestClass]
    public class PmfTests
    {
        [TestMethod]
        public void MakePmfFromListTest()
        {
            Pmf pmf = Pmf.MakePmfFromList(new decimal[] {1, 2, 2, 3, 4});

            var values = pmf.Values().ToList();
            Assert.AreEqual(4, values.Count());
            Assert.IsTrue(values.Contains(1));
            Assert.IsTrue(values.Contains(2));
            Assert.IsTrue(values.Contains(3));
            Assert.IsTrue(values.Contains(4));

            var freqs = pmf.Frequencies().ToList().Distinct().ToList();
            Assert.AreEqual(2, freqs.Count());
            Assert.IsTrue(freqs.Contains((decimal)0.2));
            Assert.IsTrue(freqs.Contains((decimal)0.4));

            Assert.AreEqual((decimal)0.2, pmf.Prob(1));
            Assert.AreEqual((decimal)0.4, pmf.Prob(2));
            Assert.AreEqual((decimal)0.2, pmf.Prob(3));
            Assert.AreEqual((decimal)0.2, pmf.Prob(4));
            Assert.AreEqual((decimal)1.0, pmf.Total());
        }

        [TestMethod]
        public void ThinkStatsSampleTest()
        {
            Pmf pmf = Pmf.MakePmfFromList(new decimal[] { 1, 2, 2, 3, 5 });
            Assert.AreEqual((decimal)0.4, pmf.Prob(2));
            pmf.Incr(2, (decimal)0.2);
            Assert.AreEqual((decimal)0.6, pmf.Prob(2));

            pmf.Mult(2, (decimal)0.5);
            Assert.AreEqual((decimal)0.3, pmf.Prob(2));

            Assert.AreEqual((decimal)0.9, pmf.Total());

            pmf.Normalize();
            Assert.AreEqual((decimal)1.0, pmf.Total());
        }

        [TestMethod]
        public void PmfMeanTests()
        {
            Pmf pmf = Pmf.MakePmfFromList(new decimal[] {1, 2, 3});
            Assert.AreEqual((decimal)2, pmf.Mean());

            pmf = Pmf.MakePmfFromList(new decimal[] { 5, 5, 20, 20, 0 });
            Assert.AreEqual((decimal)10, pmf.Mean());
        }
    }
}

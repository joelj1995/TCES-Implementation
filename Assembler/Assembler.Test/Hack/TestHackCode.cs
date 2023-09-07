using Assembler.Logic.Hack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Test.Hack
{
    public class TestHackCode
    {
        [Test]
        public void TestDest()
        {
            Assert.That(HackCode.dest(null), Is.EqualTo("000"));
            Assert.That(HackCode.dest("M"), Is.EqualTo("001"));
            Assert.That(HackCode.dest("AMD"), Is.EqualTo("111"));
        }

        [Test]
        public void TestCompTranslation()
        {
            foreach (var dest in HackCode.CompTranslation)
            {
                Assert.That(dest.Value.Length, Is.EqualTo(7));
            }
            var uniqueCount = HackCode.CompTranslation.Select(c => c.Value).Distinct().Count();
            Assert.That(uniqueCount, Is.EqualTo(HackCode.CompTranslation.Count));
        }

        [Test]
        public void TestComp()
        {
            Assert.That(HackCode.comp("M-1"), Is.EqualTo("1110010"));
            Assert.That(HackCode.comp("A-1"), Is.EqualTo("0110010"));
        }

        [Test]
        public void TestJump()
        {
            Assert.That(HackCode.jump(null), Is.EqualTo("000"));
            Assert.That(HackCode.jump("JMP"), Is.EqualTo("111"));
            Assert.That(HackCode.jump("JGE"), Is.EqualTo("011"));
        }

        [Test]
        public void TestDictDefinitionParity()
        {
            var destCodes = HackCode.DestTranslation.Select(c => c.Key);
            CollectionAssert.AreEquivalent(destCodes, HackParser.Dests.Where(d => d != null));
            var compCodes = HackCode.CompTranslation.Select(c => c.Key);
            CollectionAssert.AreEquivalent(compCodes, HackParser.Comps.Where(d => d != null));
            var jumpCodes = HackCode.JumpTranslation.Select(c => c.Key);
            CollectionAssert.AreEquivalent(jumpCodes, HackParser.Jumps.Where(d => d != null));
        }
    }
}

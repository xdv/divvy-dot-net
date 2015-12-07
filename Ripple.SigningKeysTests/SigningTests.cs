﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ripple.Crypto;

namespace Ripple.Crypto.Tests
{
    using static Ripple.Address;
    using Ripple.Utils;
    using Seed = Ripple.Crypto.Seed;
    using System.IO;
    using System;
    using Org.BouncyCastle.Utilities.Encoders;

    [TestClass()]
    public class SigningTests
    {
        byte[] message = new byte[] { 0xb, 0xe, 0xe, 0xf };

        [TestMethod()]
        public void SanityTestK256()
        {
            var keypair = Seed.FromPassPhrase("niq").KeyPair();
            var sig = keypair.Sign(message);
            Assert.IsTrue(keypair.Verify(message, sig));
        }
        [TestMethod()]
        public void SanityTestEd25519()
        {
            var keypair = Seed.FromPassPhrase("niq").SetEd25519().KeyPair();
            var sig = keypair.Sign(message);
            Assert.IsTrue(keypair.Verify(message, sig));
        }

        public string ToHex(byte[] val)
        {
            return Hex.ToHexString(val).ToUpper();
        }

        [TestMethod()]
        public void TestRfc6979Determinism()
        {
            var keypair = Seed.FromPassPhrase("niq").KeyPair();

            var fixtures = new string[]
            {
                "30440220312B2E0894B81A2E070ACE566C5DFC70CDD18E67D44E2CFEF2EB5495F7DE2DAC02205E155C0019502948C265209DFDD7D84C4A05BD2C38CEE6ECD7C33E9C9B12BEC2",
                "304402202A5860A12C15EBB8E91AA83F8E19D85D4AC05B272FC0C4083519339A7A76F2B802200852F9889E1284CF407DC7F73D646E62044C5AB432EAEF3FFF3F6F8EE9A0F24C",
                "3045022100B1658C88D1860D9F8BEB25B79B3E5137BBC2C382D08FE7A068FFC6AB8978C8040220644F64B97EA144EE7D5CCB71C2372DD730FA0A659E4C18241A80D6C915350263",
                "3045022100F3E541330FF79FFC42EB0491EDE1E47106D94ECFE3CDB2D9DD3BC0E8861F6D45022013F62942DD626D6C9731E317F372EC5C1F72885C4727FDBEE9D9321BC530D7B2",
                "3045022100998ABE378F4119D8BEE9843482C09F0D5CE5C6012921548182454C610C57A269022036BD8EB71235C4B2C67339DE6A59746B1F7E5975987B7AB99B313D124A69BB9F",
                "304402200754DE2379B3333B0BC29DB74F5E1C2F4A65FF090E2B5A52D9691A2983CE73E102204CD07D7E8A02374CA00DEDEA4B17223AC782D424EE43BC9C4355CC2D45741949",
                "3045022100D96FFA0F7D347FE655067CB985B4C13190CE66ECCA73AA305788C673F7640B7502204E6E961EE5C519288D5D3FEE637A914138E5DEBF15182D47C92AB8C301D5958A",
                "304402202FEF8C6ECB139DD942F193E75778BAD324108DA23ECA4B47698962ECFD79005302202240F584E4D7E53BD1247033429A627F18DE585ED02D70A4659381B72C4050FB",
                "30440220304E276143E54CA1C8C070DF9D285BDD1DC3CDDEEEB24E9024AAE5AF373DDBC70220604F078C46D499E6193130AA8A89C2E91F3632F08E1D387114047DEBC3BE6C18",
                "3044022068080CEBD70C2FFABDB6697D38744674336B2D6441ADD825BDE6186628148502022021AA14A3CF55231404305B420C129FD97DD5C6096E1F0046E03FCCA056D1D8E2",
                "304402205A7589B193B3F1EAAD7C9B25B29B6B586FD358FE0C44F23D43F27C55284A5AB702206AD9879DB089D33C8E10A6CB889760A725DDD963DD412072A19B7F5BE119B52D",
                "3044022001D45AF8B61EA8F782238A2C330475E7BA83353146B67BAA5BF2CD91F366A8D0022010B5065CDC83A015B508C72E4E1578BBC58964510525DBC4960F02A0D1A29A4E"
            };

            for (int i = 0; i < fixtures.Length; i++)
            {
                byte[] message = new byte[] { (byte)i };
                Assert.AreEqual(fixtures[i], ToHex(keypair.Sign(message)));
            }
        }
        [TestMethod()]
        public void TestEd25519Determinism()
        {
            var keypair = Seed.FromPassPhrase("niq")
                              .SetEd25519()
                              .KeyPair();

            var messageBytes = Hex.Decode(
                  "535458001200002280000000240000" +
                  "00016140000000000003E868400000" +
                  "000000000A7321EDD3993CDC664789" +
                  "6C455F136648B7750723B011475547" +
                  "AF60691AA3D7438E021D8114C0A5AB" +
                  "EF242802EFED4B041E8F2D4A8CC86A" +
                  "E3D18314B5F762798A53D543A014CA" +
                  "F8B297CFF8F2F937E8");

            string expectedSig =
                  "C3646313B08EED6AF4392261A31B961F" +
                  "10C66CB733DB7F6CD9EAB079857834C8" +
                  "B0334270A2C037E63CDCCC1932E08328" +
                  "82B7B7066ECD2FAEDEB4A83DF8AE6303";

            Assert.AreEqual(expectedSig, ToHex(keypair.Sign(messageBytes)));
        }
    }
}


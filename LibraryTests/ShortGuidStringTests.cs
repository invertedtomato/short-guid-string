using InvertedTomato.Encoding.ShortGuidStrings;
using System;
using Xunit;

namespace LibraryTests {
    public class ShortGuidStringTests {
        private readonly Guid Raw = new Guid(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 });
        private readonly string EncodedShort = "*AQIDBAUGBwgJCgsMDQ4P~w"; // 1 + 22
        private readonly string EncodedLegacy = "<04030201-0605-0807-090a-0b0c0d0e0fff>"; // 2 + 36
        // 15 Characters shorter

        [Fact]
        public void Encode_WithoutLabel() {
            Assert.Equal(EncodedShort, ShortGuidString.Encode(Raw));
        }
        [Fact]
        public void Encode_WithLabel() {
            Assert.Equal("Cheesecake " + EncodedShort, ShortGuidString.Encode(Raw, "Cheesecake"));
        }
        [Fact]
        public void Encode_WithLabel_Tricky() {
            Assert.Equal("* " + EncodedShort, ShortGuidString.Encode(Raw, "*"));
        }
        [Fact]
        public void Encode_Nulllabel() {
            Assert.Throws<ArgumentNullException>(() => {
                ShortGuidString.Encode(Raw, null);
            });
        }

        [Fact]
        public void Decode_WithoutLabel_Short() {
            Assert.Equal(Raw, ShortGuidString.Decode(EncodedShort));
        }
        [Fact]
        public void Decode_WithoutLabel_Legacy() {
            Assert.Equal(Raw, ShortGuidString.Decode(EncodedLegacy));
        }
        [Fact]
        public void Decode_WithLabel_Short() {
            Assert.Equal(Raw, ShortGuidString.Decode("Cheesecake " + EncodedShort));
        }
        [Fact]
        public void Decode_WithLabel_Legacy() {
            Assert.Equal(Raw, ShortGuidString.Decode("Cheesecake " + EncodedLegacy));
        }

        [Fact]
        public void Decode_WithLabel_BadPattern() {
            Assert.Throws<FormatException>(() => {
                ShortGuidString.Decode("Cheesecake " + EncodedShort + "a");
            });
            Assert.Throws<FormatException>(() => {
                ShortGuidString.Decode("Cheesecake *asdf");
            });
        }



    }
}

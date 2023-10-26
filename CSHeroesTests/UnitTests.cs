using csheroes.src.unit;

namespace CSHeroesTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void HealthCantBeNegative()
        {
            Unit unit = new();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => unit.Hp = -999);
        }

        [TestMethod]
        public void HealthCantBeMoreThenMaxHealth()
        {
            Unit unit = new();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => unit.Hp = 999);
        }

        [TestMethod]
        public void HealthSetWorkCorrectly()
        {
            Unit unit = new()
            {
                Hp = 5
            };

            Assert.AreEqual(5, unit.Hp);
        }
    }
}
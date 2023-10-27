using csheroes.src.Units;

namespace CSHeroesTests
{
    [TestClass]
    public class UnitFactoryTests
    {
        [TestMethod]
        public void UnitTemplateWithoutImplementationThrowException()
        {
            Assert.ThrowsException<UnitTemplateNotFoundException>(() => UnitFactory.GetUnitByTemplate(UnitTemplate.TEST_UNIT_WITHOUT_IMPLEMENTATION));
        }

        [TestMethod]
        public void ShouldReturnUnitWithCorrectPropertiesFromTemplate()
        {
            Unit unit = UnitFactory.GetUnitByTemplate(UnitTemplate.TEST_UNIT);

            Assert.AreEqual(123, unit.MaxHp);
            Assert.AreEqual(100, unit.Hp);
            Assert.AreEqual(33, unit.Range);
            Assert.AreEqual(50, unit.Damage);
            Assert.AreEqual(12, unit.Level);
        }
    }
}

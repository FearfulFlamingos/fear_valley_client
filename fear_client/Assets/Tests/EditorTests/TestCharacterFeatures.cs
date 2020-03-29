using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Character;
using NUnit.Framework;
using NSubstitute;

namespace EditorTests
{
    public class TestCharacterFeatures : MonoBehaviour
    {

        [Test]
        public void TestAttackRoll()
        {
            // Arrange
            IRandomNumberGenerator rng = Substitute.For<IRandomNumberGenerator>();
            rng.GetRandom(1, 20).Returns(20);
            CharacterFeatures features = new CharacterFeatures()
            {
                Rng = rng,
                AttackBonus = 10,
                DamageBonus = 10
            };

            // Act
            int expected = 30;
            int result = features.GetAttackRoll();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestDamageRoll()
        {
            // Arrange
            IRandomNumberGenerator rng = Substitute.For<IRandomNumberGenerator>();
            rng.GetRandom(1, 10).Returns(10);
            CharacterFeatures features = new CharacterFeatures()
            {
                Rng = rng,
                AttackBonus = 10,
                DamageBonus = 10,
                MaxAttackVal = 10
            };

            // Act
            int expected = 20;
            int result = features.GetDamageRoll();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestMagicAttackRoll()
        {
            // Arrange
            IRandomNumberGenerator rng = Substitute.For<IRandomNumberGenerator>();
            rng.GetRandom(1, 20).Returns(20);
            CharacterFeatures features = new CharacterFeatures()
            {
                Rng = rng
            };

            // Act
            int expected = 25;
            int result = features.GetMagicAttackRoll();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestMagicDamageRoll()
        {
            // Arrange
            IRandomNumberGenerator rng = Substitute.For<IRandomNumberGenerator>();
            rng.GetRandom(1, 12).Returns(12);
            CharacterFeatures features = new CharacterFeatures()
            {
                Rng = rng,
            };

            // Act
            int expected = 13;
            int result = features.GetMagicDamageRoll();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestDamageCharacterAtFullHealth()
        {
            // Arrange
            CharacterFeatures features = new CharacterFeatures()
            {
                Health = 10
            };

            // Act
            features.DamageCharacter(5);
            int expected = 5;

            // Assert
            Assert.AreEqual(expected, features.Health);
        }

        [Test]
        public void TestDamageCharacterToDeath()
        {
            // Arrange
            CharacterFeatures features = new CharacterFeatures()
            {
                Health = 10
            };

            // Act
            features.DamageCharacter(15);
            int expected = 0;

            // Assert
            Assert.AreEqual(expected, features.Health);
        }

        [Test]
        public void TestRNGActuallyWorks()
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();

            int result = rng.GetRandom(1, 20);

            Assert.AreEqual(1, result, 20);
        }
    }
}
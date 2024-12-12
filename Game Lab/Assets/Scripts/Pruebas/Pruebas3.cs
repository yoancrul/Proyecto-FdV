using NUnit.Framework;
using UnityEngine;
using NSubstitute;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class GameScriptsTests
{
    // 1. **TimerTest**
    public class TimerTest
    {
        private Timer timer;
        private TMP_Text mockTimerText;

        [SetUp]
        public void SetUp()
        {
            GameObject timerObject = new GameObject();
            mockTimerText = timerObject.AddComponent<TMP_Text>();
            timer = timerObject.AddComponent<Timer>();
        }

        [Test]
        public void TestTimerStartAndUpdate()
        {
            // Act
            EventManager.OnTimerStart(); // Starts the timer
            timer.Update();

            // Assert
            Assert.IsTrue(timer._isRunning);
            Assert.AreEqual(mockTimerText.text, "00:59:60");
        }

        [Test]
        public void TestTimerStop()
        {
            // Act
            EventManager.OnTimerStop();

            // Assert
            Assert.IsFalse(timer._isRunning);
        }
    }

    // 2. **EventManagerTest**
    public class EventManagerTest
    {
        [Test]
        public void TestOnTimerStart()
        {
            // Arrange
            var mock = Substitute.For<UnityAction>();
            EventManager.TimerStart += mock;

            // Act
            EventManager.OnTimerStart();

            // Assert
            mock.Received().Invoke();
        }

        [Test]
        public void TestOnTimerStop()
        {
            // Arrange
            var mock = Substitute.For<UnityAction>();
            EventManager.TimerStop += mock;

            // Act
            EventManager.OnTimerStop();

            // Assert
            mock.Received().Invoke();
        }
    }

    // 3. **TutorialManagerTest**
    public class TutorialManagerTest
    {
        private TutorialManager tutorialManager;
        private GameObject mockCanvas;

        [SetUp]
        public void SetUp()
        {
            mockCanvas = new GameObject();
            tutorialManager = mockCanvas.AddComponent<TutorialManager>();
        }

        [Test]
        public void TestContinueCase0()
        {
            // Arrange
            GameObject mockMessage = new GameObject();
            GameObject mockButton = new GameObject();

            tutorialManager.Continue(0);

            // Assert
            Assert.IsTrue(mockMessage.activeSelf);
            Assert.IsTrue(mockButton.activeSelf);
        }

        [Test]
        public void TestFin()
        {
            // Arrange
            GameObject mockMessage = new GameObject();

            tutorialManager.Fin(mockMessage);

            // Assert
            Assert.IsFalse(mockMessage.activeSelf);
        }
    }

    // 4. **TutorialTest**
    public class TutorialTest
    {
        private Tutorial tutorial;
        private GameObject player;

        [SetUp]
        public void SetUp()
        {
            player = new GameObject();
            tutorial = new GameObject().AddComponent<Tutorial>();
        }

        [Test]
        public void TestOnTriggerEnter2D()
        {
            // Arrange
            var collider = new GameObject().AddComponent<BoxCollider2D>();
            collider.gameObject.tag = "Player";

            // Act
            tutorial.OnTriggerEnter2D(collider);

            // Assert
            Assert.IsTrue(tutorial.canvas.activeSelf);
        }
    }

    // 5. **MenuPrincipalTest**
    public class MenuPrincipalTest
    {
        private MenuPrincipal menuPrincipal;
        private GameObject mockMenu;
        private PlayerInput mockPlayerInput;

        [SetUp]
        public void SetUp()
        {
            mockMenu = new GameObject();
            menuPrincipal = mockMenu.AddComponent<MenuPrincipal>();
            mockPlayerInput = Substitute.For<PlayerInput>();
        }

        [Test]
        public void TestPlayGame()
        {
            // Arrange
            mockPlayerInput.currentControlScheme.Returns("Gamepad");

            // Act
            menuPrincipal.PlayGame();

            // Assert
            Assert.AreEqual("Nivel 0", SceneManager.GetActiveScene().name);
        }

        [Test]
        public void TestQuitGame()
        {
            // Act
            menuPrincipal.QuitGame();

            // Assert
            // We can't assert the Application.Quit directly, but we could ensure the function is called without errors.
        }
    }

    // 6. **BombsTest**
    public class BombsTest
    {
        private Bombs bomb;
        private PlayerMovement mockPlayerMovement;

        [SetUp]
        public void SetUp()
        {
            GameObject bombObject = new GameObject();
            bomb = bombObject.AddComponent<Bombs>();
            mockPlayerMovement = Substitute.For<PlayerMovement>();
        }

        [Test]
        public void TestDetonarBomba()
        {
            // Act
            bomb.DetonarBomba();

            // Assert
            Assert.IsTrue(bomb.detonado);
        }

        [Test]
        public void TestOnTriggerEnter2D()
        {
            // Arrange
            var collider = new GameObject().AddComponent<BoxCollider2D>();
            collider.gameObject.tag = "Player";

            // Act
            bomb.OnTriggerEnter2D(collider);

            // Assert
            Assert.AreEqual(mockPlayerMovement.bombasDisponibles, 0); // Example of assertion
        }
    }

    // 7. **CaidaTest**
    public class CaidaTest
    {
        private Caida caida;
        private GameManager mockGameManager;

        [SetUp]
        public void SetUp()
        {
            GameObject caidaObject = new GameObject();
            caida = caidaObject.AddComponent<Caida>();
            mockGameManager = Substitute.For<GameManager>();
            GameObject.FindGameObjectWithTag("GameManager").returns(mockGameManager);
        }

        [Test]
        public void TestOnCollisionEnter2DPlayerDie()
        {
            // Arrange
            var collider = new GameObject().AddComponent<BoxCollider2D>();
            collider.gameObject.tag = "Player";

            // Act
            caida.OnCollisionEnter2D(new Collision2D());

            // Assert
            mockGameManager.Received().Muere();
        }
    }

    // 8. **FondoMovimientoTest**
    public class FondoMovimientoTest
    {
        private FondoMovimiento fondoMovimiento;

        [SetUp]
        public void SetUp()
        {
            GameObject fondoObject = new GameObject();
            fondoMovimiento = fondoObject.AddComponent<FondoMovimiento>();
        }

        [Test]
        public void TestUpdate()
        {
            // Arrange
            Rigidbody2D mockRigidbody = new GameObject().AddComponent<Rigidbody2D>();
            mockRigidbody.velocity = new Vector2(1, 0);

            // Act
            fondoMovimiento.Update();

            // Assert
            Assert.AreEqual(fondoMovimiento.material.mainTextureOffset.x, 0.01f);
        }
    }

    // 9. **GameManagerTest**
    public class GameManagerTest
    {
        private GameManager gameManager;

        [SetUp]
        public void SetUp()
        {
            gameManager = new GameObject().AddComponent<GameManager>();
        }

        [Test]
        public void TestPauseGame()
        {
            // Arrange
            var callbackContext = Substitute.For<InputAction.CallbackContext>();

            // Act
            gameManager.PauseOrResumeGame(callbackContext);

            // Assert
            Assert.AreEqual(Time.timeScale, 0);
        }
    }
}

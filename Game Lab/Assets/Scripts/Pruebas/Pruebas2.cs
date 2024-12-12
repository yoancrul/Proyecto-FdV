using NUnit.Framework;
using UnityEngine;
using NSubstitute;
using UnityEngine.SceneManagement;

public class GameScriptsTests
{
    // Trampa Test
    public class TrampaTest
    {
        private Trampa trampa;
        private PlayerMovement mockPlayerMovement;
        private GameObject player;

        [SetUp]
        public void SetUp()
        {
            player = new GameObject();
            mockPlayerMovement = Substitute.For<PlayerMovement>();
            player.AddComponent<PlayerMovement>().returns(mockPlayerMovement);

            trampa = player.AddComponent<Trampa>();
            trampa.Start();
        }

        [Test]
        public void TestOnTriggerEnter2D_PlayerMuere()
        {
            // Arrange
            var collider = new GameObject().AddComponent<BoxCollider2D>();
            collider.gameObject.tag = "Player";
            
            // Act
            trampa.OnTriggerEnter2D(collider);

            // Assert
            mockPlayerMovement.Received().Muere();
        }
    }

    // ZonaRestriccionBombas Test
    public class ZonaRestriccionBombasTest
    {
        private ZonaRestriccionBombas zona;
        private PlayerMovement mockPlayerMovement;
        private GameObject player;

        [SetUp]
        public void SetUp()
        {
            player = new GameObject();
            mockPlayerMovement = Substitute.For<PlayerMovement>();
            player.AddComponent<PlayerMovement>().returns(mockPlayerMovement);

            zona = player.AddComponent<ZonaRestriccionBombas>();
        }

        [Test]
        public void TestOnTriggerEnter2D_PlayerRestrictsBombas()
        {
            // Arrange
            var collider = new GameObject().AddComponent<BoxCollider2D>();
            collider.gameObject.tag = "Player";
            mockPlayerMovement.bombasMaximas = 5;
            mockPlayerMovement.bombasDisponibles = 3;

            // Act
            zona.OnTriggerEnter2D(collider);

            // Assert
            Assert.AreEqual(0, mockPlayerMovement.bombasMaximas);
            Assert.AreEqual(0, mockPlayerMovement.bombasDisponibles);
        }

        [Test]
        public void TestOnTriggerExit2D_PlayerRestoresBombas()
        {
            // Arrange
            var collider = new GameObject().AddComponent<BoxCollider2D>();
            collider.gameObject.tag = "Player";
            mockPlayerMovement.bombasMaximas = 5;
            mockPlayerMovement.bombasDisponibles = 3;

            zona.OnTriggerEnter2D(collider); // Se activa la zona
            zona.OnTriggerExit2D(collider);  // El jugador sale de la zona

            // Assert
            Assert.AreEqual(5, mockPlayerMovement.bombasMaximas);
            Assert.AreEqual(3, mockPlayerMovement.bombasDisponibles);
        }
    }

    // BloqueActivado Test
    public class BloqueActivadoTest
    {
        private BloqueActivado bloque;
        private GameObject puertaObj;
        private Puerta mockPuerta;
        private GameObject bomba;

        [SetUp]
        public void SetUp()
        {
            bloque = new GameObject().AddComponent<BloqueActivado>();
            puertaObj = new GameObject();
            mockPuerta = Substitute.For<Puerta>();
            puertaObj.AddComponent<Puerta>().returns(mockPuerta);

            bloque.puertaAsociada = mockPuerta;
            bomba = new GameObject();
            bomba.tag = "bomba";
        }

        [Test]
        public void TestOnTriggerEnter2D_ActivateBlockAndOpenDoor()
        {
            // Arrange
            var collider = bomba.AddComponent<BoxCollider2D>();

            // Act
            bloque.OnTriggerEnter2D(collider);

            // Assert
            mockPuerta.Received().Abrir();
        }
    }

    // BombasRecogibles Test
    public class BombasRecogiblesTest
    {
        private BombasRecogibles bombasRecogibles;
        private GameObject player;
        private PlayerMovement mockPlayerMovement;

        [SetUp]
        public void SetUp()
        {
            player = new GameObject();
            mockPlayerMovement = Substitute.For<PlayerMovement>();
            player.AddComponent<PlayerMovement>().returns(mockPlayerMovement);

            bombasRecogibles = player.AddComponent<BombasRecogibles>();
        }

        [Test]
        public void TestOnTriggerEnter2D_IncreaseMaxBombs()
        {
            // Arrange
            var collider = new GameObject().AddComponent<BoxCollider2D>();
            collider.gameObject.tag = "Player";

            // Act
            bombasRecogibles.OnTriggerEnter2D(collider);

            // Assert
            mockPlayerMovement.Received().AumentarBombasMaximas();
        }
    }

    // Plataforma Atra Test
    public class PlataformaAtraTest
    {
        private PLataformaatra plataforma;
        private GameObject player;
        private Collider2D mockCollider;

        [SetUp]
        public void SetUp()
        {
            player = new GameObject();
            plataforma = new GameObject().AddComponent<PLataformaatra>();
            mockCollider = Substitute.For<Collider2D>();
        }

        [Test]
        public void TestOnCollisionStay2D_DisableCollider()
        {
            // Arrange
            var collision = new GameObject().AddComponent<BoxCollider2D>();
            collision.gameObject.tag = "Player";

            // Act
            plataforma.OnCollisionStay2D(new Collision2D());

            // Assert
            Assert.AreEqual(false, plataforma.GetComponent<Collider2D>().enabled);
        }
    }

    // Puerta Test
    public class PuertaTest
    {
        private Puerta puerta;

        [SetUp]
        public void SetUp()
        {
            puerta = new GameObject().AddComponent<Puerta>();
        }

        [Test]
        public void TestAbrirPuerta()
        {
            // Act
            puerta.Abrir();

            // Assert
            Assert.AreEqual(true, puerta.gameObject.activeInHierarchy);
        }
    }

    // PuertaNivel Test
    public class PuertaNivelTest
    {
        private PuertaNivel puertaNivel;

        [SetUp]
        public void SetUp()
        {
            puertaNivel = new GameObject().AddComponent<PuertaNivel>();
        }

        [Test]
        public void TestOnTriggerEnter2D_LoadScene()
        {
            // Arrange
            var collider = new GameObject().AddComponent<BoxCollider2D>();
            collider.gameObject.tag = "Player";
            
            // Act
            puertaNivel.OnTriggerEnter2D(collider);

            // Assert
            Assert.AreEqual(true, SceneManager.GetActiveScene().name.Contains("Nivel"));
        }
    }
}

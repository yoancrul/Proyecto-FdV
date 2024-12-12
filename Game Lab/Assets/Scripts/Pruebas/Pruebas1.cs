using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UnitTests
{
    [Test]
    public void EnemyLogic_MoveLeft_WhenNotPushedOrFalling()
    {
        // Arrange
        GameObject enemy = new GameObject();
        EnemyLogic enemyLogic = enemy.AddComponent<EnemyLogic>();
        Rigidbody2D rb = enemy.AddComponent<Rigidbody2D>();
        enemyLogic.velocidad = 2f;
        enemyLogic.siendoEmpujado = false;
        enemyLogic.cayendo = false;

        // Act
        enemyLogic.Update();

        // Assert
        Assert.AreEqual(-2f, rb.velocity.x, "El enemigo no se mueve a la izquierda correctamente.");
    }

    [Test]
    public void FlyingEnemy_ChangesDirectionAtLimits()
    {
        // Arrange
        GameObject enemy = new GameObject();
        FlyingEnemy flyingEnemy = enemy.AddComponent<FlyingEnemy>();
        flyingEnemy.velocidad = 1f;
        Transform limiteSuperior = new GameObject().transform;
        Transform limiteInferior = new GameObject().transform;
        limiteSuperior.position = new Vector3(0, 5, 0);
        limiteInferior.position = new Vector3(0, -5, 0);
        flyingEnemy.limiteSuperior = limiteSuperior;
        flyingEnemy.limiteInferior = limiteInferior;

        // Act
        enemy.transform.position = new Vector3(0, 5, 0);
        flyingEnemy.Update();

        // Assert
        Assert.IsFalse(flyingEnemy.moviendoHaciaArriba, "El enemigo volador no cambia correctamente de dirección al alcanzar el límite superior.");
    }

    [Test]
    public void PlayerMovement_JumpOnlyWhenGrounded()
    {
        // Arrange
        GameObject player = new GameObject();
        PlayerMovement playerMovement = player.AddComponent<PlayerMovement>();
        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        BoxCollider2D coll = player.AddComponent<BoxCollider2D>();
        playerMovement.coll = coll;
        playerMovement.fuerzaSalto = 10f;

        // Act
        playerMovement.Jump(new InputAction.CallbackContext());

        // Assert
        Assert.AreEqual(10f, rb.velocity.y, "El jugador no salta correctamente cuando está en el suelo.");
    }

    [Test]
    public void ManejoBombas_GeneratesBombCorrectly()
    {
        // Arrange
        GameObject player = new GameObject();
        ManejoBombas manejoBombas = player.AddComponent<ManejoBombas>();
        GameObject bombPrefab = new GameObject();
        manejoBombas.bombPrefab = bombPrefab;
        manejoBombas.playerMovement = player.AddComponent<PlayerMovement>();
        manejoBombas.playerMovement.bombasDisponibles = 1;

        // Act
        manejoBombas.GeneratePreciseBomb(new InputAction.CallbackContext());

        // Assert
        Assert.IsNotNull(manejoBombas.bomba, "La bomba no se genera correctamente.");
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

public class PlayerMovementTest
{
    internal class IsGroundedTest
    {
        public void NotGrounded1()
        {
            GameObject go = new GameObject();
            var pl = go.GetComponent<PlayerMovement>();
            go.GetComponent<Collider2D>();

            Assert.IsFalse(pl.IsGrounded());
        }

        public void NotGrounded2()
        {
            GameObject go = new GameObject();
            GameObject suelo = new GameObject();

            var pl = go.AddComponent<PlayerMovement>();
            go.GetComponent<Collider2D>();
            var pospl = go.AddComponent<Transform>();
            pospl.SetPositionAndRotation(new Vector2(0, 0), new Quaternion());

            var coll = suelo.AddComponent<Collider2D>();
            var possuelo = suelo.AddComponent<Transform>();
            possuelo.SetPositionAndRotation(new Vector2(50, 50), new Quaternion());

            Assert.IsFalse(pl.IsGrounded());
        }

        public void Grounded()
        {
            GameObject go = new GameObject();
            GameObject suelo = new GameObject();

            var pl = go.AddComponent<PlayerMovement>();
            go.GetComponent<Collider2D>();
            var pospl = go.AddComponent<Transform>();
            pospl.SetPositionAndRotation(new Vector2(0, 0), new Quaternion());

            var coll = suelo.AddComponent<Collider2D>();
            var possuelo = suelo.AddComponent<Transform>();
            possuelo.SetPositionAndRotation(new Vector2(0, 0), new Quaternion());

            Assert.IsTrue(pl.IsGrounded());
        }
    }

    class IsFallingTest
    {
        public void NotFalling()
        {
            GameObject go = new GameObject();
            go.AddComponent<Rigidbody2D>();
            var pl = go.AddComponent<PlayerMovement>();
            Assert.IsFalse(pl.IsFalling());
        }
        public IEnumerator Falling()
        {
            GameObject go = new GameObject();
            go.AddComponent<Rigidbody2D>();
            var pl = go.AddComponent<PlayerMovement>();
            yield return new WaitForSeconds(1);
            Assert.IsTrue(pl.IsFalling());
        }
    }

    class BombasTest
    {
        public void AumentarBombasMaximasTest()
        {
            GameObject go = new GameObject();
            var s = go.AddComponent<PlayerMovement>();
            var res = s.bombasMaximas++;
            s.AumentarBombasMaximas();

            Assert.AreEqual(res, s.bombasMaximas);
        }
        public void AumentarBombaDisponibleTest()
        {
            GameObject go = new GameObject();
            var s = go.AddComponent<PlayerMovement>();
            var res = s.bombasDisponibles++;
            s.AumentarBombaDisponible();

            Assert.AreEqual(res, s.bombasDisponibles);
        }
        public void QuitarBombaDisponibleTest()
        {
            GameObject go = new GameObject();
            var s = go.AddComponent<PlayerMovement>();
            var res = s.bombasDisponibles--;
            s.QuitarBombaDisponible();

            Assert.AreEqual(res, s.bombasDisponibles);
        }
        public void IgualarBombasTest()
        {
            GameObject go = new GameObject();
            var s = go.AddComponent<PlayerMovement>();
            s.AumentarBombasMaximas();
            s.AumentarBombasMaximas();
            s.AumentarBombasMaximas();
            s.QuitarBombaDisponible();
            s.QuitarBombaDisponible();

            s.IgualarBombas();

            Assert.AreEqual(s.bombasMaximas, s.bombasDisponibles);
        }

    }

}
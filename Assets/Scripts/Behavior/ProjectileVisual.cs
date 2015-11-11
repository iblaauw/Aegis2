using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Aegis;

public class ProjectileVisual : MonoBehaviour
{
    public float speed;
    public bool shouldRotate;

    public IGridSquare StartSquare { get; set; }

    public IGridSquare Target { get; set; }

    public event Action Finished;

    void Start() {
        if (Target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        SquareMovement moveController = this.GetComponentForce<SquareMovement>();
        moveController.SetPosition(StartSquare);
        moveController.LongMoveFinished += (s) => FireFinished();
        moveController.StartMove(Target, speed);

        if (shouldRotate)
            this.transform.Rotate(0, 0, moveController.Rotation);
    }

    void OnDestroy() {
        this.Finished = null;
    }

    private void FireFinished()
    {
        if (this.Finished != null)
            this.Finished();

        Destroy(this.gameObject);
    }
}

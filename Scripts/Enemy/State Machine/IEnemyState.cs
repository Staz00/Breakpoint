using UnityEngine;
using System.Collections;

public interface IEnemyState {

    void UpdateState();

    void OnTriggerEnter(Collider other);

    void ToRoamState();

    void ToAlertState();

    void ToChaseState();

    void ToNeutralState();

    void ToPassiveState();
}

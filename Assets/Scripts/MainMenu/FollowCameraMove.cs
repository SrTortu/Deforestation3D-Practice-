using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class FollowCameraMove : MonoBehaviour
{
    // El objeto alrededor del cual orbitaremos
    [SerializeField] private Transform _orbitTarget;
    
    // La duración de una órbita completa en segundos
    [SerializeField] private float _orbitDuration = 20.0f; 
    
    // La distancia de la cámara al objetivo (se usará para la posición inicial)
    [SerializeField] private float _distance = 20.0f; 

    void Start()
    {
        
    }

    void Update()
    {
       
        
     
        Camera.main.transform.RotateAround(_orbitTarget.position, Vector3.up,_orbitDuration * Time.deltaTime);
        Camera.main.transform.LookAt(_orbitTarget);
    }
}
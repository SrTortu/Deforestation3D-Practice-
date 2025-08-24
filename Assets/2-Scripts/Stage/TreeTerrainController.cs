using UnityEngine;
using System.Collections.Generic;

namespace Deforestation
{

    public class TreeTerrainController : MonoBehaviour
    {
       #region Properties
       public TreeInstance[] Trees => _trees;
       #endregion

       #region Fields
       [SerializeField] private Tree _treeDetectionPrefab;
       [SerializeField] private Tree _treePrefab;
       private TreeInstance[] _trees;
       private Tree[] _treeDetectors; // Variable para guardar los detectores
       Terrain _terrain;
       #endregion

       #region Unity Callbacks
       void Start()
       {
          _terrain = Terrain.activeTerrain;
          _trees = _terrain.terrainData.treeInstances;

          InitializeTrees();
       }

       private void InitializeTrees()
       {
          _treeDetectors = new Tree[_trees.Length]; // Inicializa el array de detectores
          for (int i = _trees.Length - 1; i >= 0; i--)
          {
             TreeInstance tree = _trees[i];
             Vector3 treeWorldPos = TreeToWorldPosition(tree);
             Tree treeDetector = Instantiate(_treeDetectionPrefab, treeWorldPos, Quaternion.identity);
             treeDetector.transform.parent = transform;
             treeDetector.Index = i;
             _treeDetectors[i] = treeDetector; // Guarda la referencia del detector
          }
       }

       public GameObject DestroyTree(int i, Vector3 treeWorldPos)
       {
          Tree newTree = Instantiate(_treePrefab, treeWorldPos, Quaternion.identity);
          RemoveTreeFromTerrain(i);
          return newTree.gameObject;
       }
       
       void OnDestroy()
       {
          _terrain.terrainData.treeInstances = _trees;
       }
       #endregion

       #region Public Methods
       public Vector3 TreeToWorldPosition(TreeInstance tree)
       {
          return Vector3.Scale(tree.position, _terrain.terrainData.size) + _terrain.transform.position;
       }

       public void RemoveTreeFromTerrain(int index)
       {
          // TODO: Reasignar todos los indices de todos los tree detectors.
          // El TODO en tu código indica que sabías que había un problema. ¡Aquí está la solución!

          // Primero, elimina el detector de árbol que ya no necesitas
          Destroy(_treeDetectors[index].gameObject);

          // Ahora, elimina el árbol del TerrainData
          List<TreeInstance> trees = new List<TreeInstance>(_terrain.terrainData.treeInstances);
          trees.RemoveAt(index);
          _terrain.terrainData.treeInstances = trees.ToArray();

          // Actualiza la referencia del array de detectores
          List<Tree> detectors = new List<Tree>(_treeDetectors);
          detectors.RemoveAt(index);
          _treeDetectors = detectors.ToArray();

          // Actualiza todos los índices de los detectores restantes
          for (int i = 0; i < _treeDetectors.Length; i++)
          {
             _treeDetectors[i].Index = i;
          }
       }
       #endregion
       
    }
}
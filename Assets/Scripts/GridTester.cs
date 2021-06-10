using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTester : MonoBehaviour
{
    //[SerializeField] private LayerMask _whatIsGround; 
    //private ObstacleGrid _grid;
    //private Camera _camera;
    //void Start()
    //{
    //    _grid = new ObstacleGrid(20, 20, transform.position);
    //    _camera = Camera.main;
    //}
    //private void OnDrawGizmos()
    //{
    //    DrawGrid();
    //}

    //private void Update()
    //{
    //    if(Input.GetButtonDown("Fire1"))
    //    {
    //        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
    //        if(Physics.Raycast(ray, out RaycastHit hit, 100))
    //        {
    //            Debug.Log(hit.point);
    //            _grid.CanGetSellByPos(hit.point, out Vector2Int vector);
    //            Debug.Log(vector);
    //        }    
    //    }
    //}


    //private void DrawGrid()
    //{
    //    if (_grid == null)
    //        return;

    //    Gizmos.color = Color.red;
    //    for (int x = 0; x < _grid.Width; x++)
    //    {
    //        for (int y = 0; y < _grid.Height; y++)
    //        {
    //            Gizmos.DrawLine(_grid.GetCellStartPos(x, y), _grid.GetCellStartPos(x, y + 1));
    //            Gizmos.DrawLine(_grid.GetCellStartPos(x, y), _grid.GetCellStartPos(x + 1, y));
    //        }
    //    }
    //    Gizmos.DrawLine(_grid.GetCellStartPos(0, _grid.Height), _grid.GetCellStartPos(_grid.Width, _grid.Height));
    //    Gizmos.DrawLine(_grid.GetCellStartPos(_grid.Width, 0), _grid.GetCellStartPos(_grid.Width, _grid.Height));
    //}
}

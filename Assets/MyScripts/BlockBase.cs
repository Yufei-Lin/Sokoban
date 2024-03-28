using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blocks
{
    public abstract class BlockBase : MonoBehaviour
    {
        #region housekeeping
        public GridObject gridObject;
        private bool triggered = false;

        // easy access to blocks
        public static Dictionary<Vector2Int, BlockBase> CoordinateToBlock = new Dictionary<Vector2Int, BlockBase>();

        private void Start()
        {
            gridObject = GetComponent<GridObject>();

            CoordinateToBlock.Add(gridObject.gridPosition, this);
        }

        private void OnDisable()
        {
            CoordinateToBlock.Remove(gridObject.gridPosition);
        }

        protected Vector2Int[] adjacents = new Vector2Int[]
        {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
        };

        public enum MoveType
        {
            None,
            Push,
            Pull,
            Sticky, // basically adjacent but neither push nor pull
        }

        protected bool InBounds(Vector2Int position)
        {
            return position.x >= 1 && position.x < GridMaker.reference.dimensions.x + 1 &&
                   position.y >= 1 && position.y < GridMaker.reference.dimensions.y + 1;
        }

        #endregion

        public abstract bool CanMove(Vector2Int direction, MoveType moveType);

        public void AttemptMove(Vector2Int direction, MoveType moveType)
        {
            if (triggered) { return; }
            // set true immediately to avoid stack overflow
            triggered = true;

            if (!InBounds(gridObject.gridPosition + direction)) { return; }


            if (CanMove(direction, moveType))
            {
                // boardcast first then actually move
                // Note: original method was queuing up all moves then execute at once,
                // but directly casading seems more intuitive
                BoardcastAdjacent(direction, gridObject.gridPosition);
                MoveSelf(direction);
                StartCoroutine(ManipulateDictAwait(gridObject.gridPosition, gridObject.gridPosition - direction, this));
            }
            else
            {
                // then set back to false if no moving is done
                triggered = false;
            }
        }

        // use WaitUntil to avoid key conflict, might exist more elegant approach
        IEnumerator ManipulateDictAwait(Vector2Int coordinate, Vector2Int originalCoordinate, BlockBase block)
        {
            yield return new WaitUntil(() => !CoordinateToBlock.ContainsKey(coordinate));
            CoordinateToBlock.Add(coordinate, block);
            CoordinateToBlock.Remove(originalCoordinate);
        }

        protected void BoardcastAdjacent(Vector2Int direction, Vector2Int originalCoordinate)
        {
            foreach (Vector2Int adjacent in adjacents)
            {
                if (direction == -adjacent)
                {
                    if (CoordinateToBlock.ContainsKey(originalCoordinate + adjacent))
                    {
                        BlockBase block = CoordinateToBlock[originalCoordinate + adjacent];
                        Debug.Log($"{name} boardcasting {direction}, pull to {block.name}");
                        block.AttemptMove(direction, MoveType.Pull);
                    }
                }
                else if (direction == adjacent)
                {
                    if (CoordinateToBlock.ContainsKey(originalCoordinate + adjacent))
                    {
                        BlockBase block = CoordinateToBlock[originalCoordinate + adjacent];
                        Debug.Log($"{name} boardcasting {direction}, push to {block.name}");
                        block.AttemptMove(direction, MoveType.Push);
                    }
                }
                else
                {
                    if (CoordinateToBlock.ContainsKey(originalCoordinate + adjacent))
                    {
                        BlockBase block = CoordinateToBlock[originalCoordinate + adjacent];
                        Debug.Log($"{name} boardcasting {direction}, sticky to {block.name}");
                        block.AttemptMove(direction, MoveType.Sticky);
                    }

                }
            }
        }

        private void MoveSelf(Vector2Int direction)
        {
            gridObject.gridPosition += direction;
        }

        protected BlockBase GetBlockInDirection(Vector2Int direction)
        {
            if (CoordinateToBlock.ContainsKey(gridObject.gridPosition + direction))
            {
                return CoordinateToBlock[gridObject.gridPosition + direction];
            }
            return null;
        }

        // really not the most optimal way
        // passing moveType happens to works for the currently needed features
        // but I can see it breaking with more feature requests
        protected bool CheckHeadingBlock(Vector2Int direction, MoveType moveType)
        {
            BlockBase frontBlock = GetBlockInDirection(direction);

            if (frontBlock == null)
            {
                return true;
            }
            else
            {
                bool wallCheck = frontBlock.InBounds(frontBlock.gridObject.gridPosition + direction);
                if (!wallCheck)
                {
                    return false;
                }

                bool moveCheck = frontBlock.CanMove(direction, moveType);
                return moveCheck;
            }
        }


        public void ResetTrigger()
        {
            triggered = false;
        }
    }

}

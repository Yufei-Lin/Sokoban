using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blocks
{
    public class PlayerBlock : BlockBase
    {
        public override bool CanMove(Vector2Int direction, MoveType moveType)
        {
            return CheckHeadingBlock(direction, MoveType.Push);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                // don't wanna mess with the coordinate system, so just simply flip Y
                PlayerMove(Vector2Int.down);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                PlayerMove(Vector2Int.up);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                PlayerMove(Vector2Int.left);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                PlayerMove(Vector2Int.right);
            }
        }

        void PlayerMove(Vector2Int direction)
        {
            SetAllBlocksToTriggered();
            AttemptMove(direction, MoveType.None);
        }

        private void SetAllBlocksToTriggered()
        {
            foreach (var block in CoordinateToBlock.Values)
            {
                block.ResetTrigger();
            }
        }
    }

}

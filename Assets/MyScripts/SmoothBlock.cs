using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Blocks
{
    public class SmoothBlock : BlockBase
    {
        // works as a normal box that can be only pushed
        public override bool CanMove(Vector2Int direction, MoveType moveType)
        {
            if (moveType != MoveType.Push)
            {
                return false;
            }

            return CheckHeadingBlock(direction, MoveType.Push);
        }
    }

}

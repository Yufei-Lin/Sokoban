using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blocks
{
    public class StickyBlock : BlockBase
    {
        // stick to anything (as long as destination is valid)
        public override bool CanMove(Vector2Int direction, MoveType moveType)
        {
            if (moveType != MoveType.Pull)
            {
                return CheckHeadingBlock(direction, MoveType.Push);
            }
            else
            {
                return true;
            }
        }
    }
}


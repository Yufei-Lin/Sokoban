using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blocks
{
    public class ClingyBlock : BlockBase
    {
        // works as a box that can be only pulled
        public override bool CanMove(Vector2Int direction, MoveType moveType)
        {
            if (moveType != MoveType.Pull)
            {
                return false;
            }

            return CheckHeadingBlock(direction, MoveType.Pull);
        }
    }

}

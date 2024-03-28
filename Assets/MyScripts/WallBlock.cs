using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blocks
{
    public class WallBlock : BlockBase
    {
        public override bool CanMove(Vector2Int direction, MoveType moveType)
        {
            return false;
        }
    }

}
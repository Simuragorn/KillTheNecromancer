using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Constants
{
    public enum UnitEnum
    {
        //Player
        DamagedSkeleton = 1,
        DarkArcher = 2,

        //Enemies
        Knight = 10,
    }

    public static class UnitEnumExtensions
    {
        public static bool IsPlayerUnit(this UnitEnum unitEnum)
        {
            return unitEnum == UnitEnum.DamagedSkeleton;
        }
    }
}

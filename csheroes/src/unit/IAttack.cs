using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csheroes.src.unit
{
    interface IAttack
    {
        void Attack(Unit unit, Unit enemy);
    }

    class MeleeAttack : IAttack
    {
        public MeleeAttack()
        {

        }

        public void Attack(Unit unit, Unit enemy)
        {
            throw new NotImplementedException();
        }
    }
}

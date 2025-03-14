using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaMedica.Modelos.Base
{
    public abstract class Entidade
    {
        public Entidade()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}

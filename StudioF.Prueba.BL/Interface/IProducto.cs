using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StudioF.Prueba.BL.Interface
{
    public interface IProducto
    {
        public Task<AdminRespuesta> GetCausalesNoLecturaQrByQuery(ParametroCausalNolecturaQr parametros);

    }
}

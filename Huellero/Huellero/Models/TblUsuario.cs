using System;
using System.Collections.Generic;

namespace Huellero.Models
{
    public partial class TblUsuario
    {
        public int Id { get; set; }
        public string? VNombres { get; set; }
        public string? VApellidos { get; set; }
        public string? VHuella { get; set; }
    }
}

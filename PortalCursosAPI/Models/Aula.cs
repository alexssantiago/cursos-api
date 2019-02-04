using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalCursosAPI.Models
{
    [Table("Aulas")]
    public class Aula
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título da aula deve ser preenchido.")]
        [MaxLength(50, ErrorMessage = "O título da aula deve ter até 50 caracteres.")]
        [MinLength(10, ErrorMessage = "O título da aula deve ter no mínimo 10 caracteres.")]
        public string Titulo { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "A ordem da aula deve ser maior que zero.")]
        public int Ordem { get; set; }

        [JsonIgnore]
        [ForeignKey("Curso")]
        public int IdCurso { get; set; }

        [JsonIgnore]
        public virtual Curso Curso { get; set; }

    }
}
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Model
{
    public abstract class Base
    {
       
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DataCadastro { get;  set; } = DateTime.Now;
    }
}

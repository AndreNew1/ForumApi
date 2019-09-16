using Microsoft.EntityFrameworkCore;

namespace Model
{
    public class BancoContexto:DbContext
    {

        public BancoContexto(DbContextOptions options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; } 
        public DbSet<Comentarios> Comentarios { get; set; }
        public DbSet<Publicacao> Publicacaos { get; set; }
    }
}

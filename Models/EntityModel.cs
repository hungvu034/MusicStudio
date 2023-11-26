
using Microsoft.EntityFrameworkCore ; 
namespace BTL.Models
{
    public class EntityModel : DbContext
    {
       public DbSet<Audio> Audios{ get; set ;}
       public DbSet<DateControl> Controls { get; set ;}
        public DbSet<AudioDate> AudioDates { get; set ;}
       public EntityModel(){
           
       }

        public EntityModel(DbContextOptions<EntityModel> Options) : base(Options){
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Audio>(
                builder => {
                    builder.HasMany(audio => audio.AudioDates)
                            .WithOne(audioDates => audioDates.Audio)
                            .HasForeignKey(audioDate => audioDate.AudioID)
                            .OnDelete(DeleteBehavior.Cascade);
                    builder.Property(A => A.ID).ValueGeneratedOnAdd();
                }
            );
            modelBuilder.Entity<DateControl>(
                builder => {
                    builder.Property(A => A.ID).ValueGeneratedOnAdd();
                    builder.HasMany(date => date.AudioDates)
                            .WithOne(audiodate => audiodate.Date)
                            .HasForeignKey(audiodate => audiodate.DateID)
                            .OnDelete(DeleteBehavior.Cascade);
                }
            );
            modelBuilder.Entity<AudioDate>(
                builder => {
                    builder.Property(AD => AD.ID).ValueGeneratedOnAdd();
                }
            );
        }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ChatModels.Entities;

public abstract class BaseEntity {
    [Key]
    [Column("id")]
    public int Id { get; set; }
}
